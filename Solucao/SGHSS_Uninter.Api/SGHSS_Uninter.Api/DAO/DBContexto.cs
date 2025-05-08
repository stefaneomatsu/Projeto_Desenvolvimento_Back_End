using FirebirdSql.Data.FirebirdClient;
using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.Persistente;
using System.Collections;
using System.Data;
using System.Reflection;

namespace SGHSS_Uninter.Api.DAO
{
    public class DBContexto<T> : IDisposable where T : new()
    {
        private readonly string _connectionString;
        private string _sql = string.Empty;
        private Dictionary<string, object> _parametros = new Dictionary<string, object>();
        private bool _disposed = false;

        protected IConfiguration Configuration { get; }

        public DBContexto(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connectionString = Configuration.GetConnectionString("FirebirdConnection") ??
                throw new InvalidOperationException("Connection string 'FirebirdConnection' not found.");
        }

        protected string SQL
        {
            get => _sql;
            set => _sql = value ?? throw new ArgumentNullException(nameof(value));
        }

        protected Dictionary<string, object> Parametros
        {
            get => _parametros;
            set => _parametros = value ?? throw new ArgumentNullException(nameof(value));
        }

        protected string ColunasFormatada => string.Join(",", Colunas());

        public List<string> Colunas()
        {
            return typeof(T).GetProperties()
                .Select(prop => prop.GetCustomAttribute<ColunaBD>()?.Nome)
                .Where(nome => !string.IsNullOrEmpty(nome))
                .ToList();
        }

        protected async Task<int> ExecutarComando()
        {
            using var connection = new FbConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new FbCommand(SQL, connection);

            if (Parametros?.Count > 0)
            {
                foreach (var param in Parametros)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
                Parametros.Clear();
            }

            return await command.ExecuteNonQueryAsync();
        }

        protected async Task<List<T>> ExecutarConsulta()
        {
            return await ExecutarConsulta<T>();
        }

        protected async Task<List<T>> ExecutarConsulta<T>() where T : new()
        {
            using var connection = new FbConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new FbCommand(SQL, connection);

            if (Parametros?.Count > 0)
            {
                foreach (var param in Parametros)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
                Parametros.Clear();
            }

            using var reader = await command.ExecuteReaderAsync();
            var dataTable = new DataTable();
            dataTable.Load(reader);

            var retorno = await Mapear<T>(dataTable);
            await TratarAssociacoes(retorno);
            return retorno;
        }
        private async Task TratarAssociacoes<T>(List<T> retorno) where T : new()
        {
            if (retorno == null || retorno.Count == 0)
                return;

            foreach (var item in retorno)
            {
                foreach (var prop in typeof(T).GetProperties())
                {
                    var atributoAssociacao = prop.GetCustomAttribute<AssociacaoBD>();
                    if (atributoAssociacao == null)
                        continue;

                    try
                    {
                        var chavePrincipal = (item as BaseObjetoPersistente).Chave;
                        if (chavePrincipal == null)
                            continue;

                        // Verifica se é uma coleção (associação one-to-many)
                        if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) &&
                            prop.PropertyType != typeof(string) &&
                            prop.PropertyType.IsGenericType)
                        {
                            await TratarAssociacaoLista(item, prop, atributoAssociacao, chavePrincipal);
                        }
                        else // Associação simples (one-to-one ou many-to-one)
                        {
                            await TratarAssociacaoSimples(item, prop, atributoAssociacao, chavePrincipal);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(
                            $"Erro ao carregar associação {prop.Name} em {typeof(T).Name}", ex);
                    }
                }
            }
        }

        private async Task TratarAssociacaoSimples<T>(T item, PropertyInfo prop, AssociacaoBD atributoAssociacao, string chavePrincipal)
        {
            // 1. Obter a chave do objeto associado
            var chaveAssociada = await ObterChaveAssociada(atributoAssociacao, chavePrincipal);
            if (chaveAssociada == null)
                return;

            // 2. Carregar o objeto associado completo
            var objetoAssociado = await CarregarObjetoAssociado(prop.PropertyType, chaveAssociada);
            prop.SetValue(item, objetoAssociado);
        }

        private async Task TratarAssociacaoLista<T>(T item, PropertyInfo prop, AssociacaoBD atributoAssociacao, string chavePrincipal)
        {
            // 1. Obter o tipo dos itens da lista
            var tipoItemLista = prop.PropertyType.GetGenericArguments()[0];

            // 2. Obter todas as chaves associadas
            var chavesAssociadas = await ObterChavesAssociadas(atributoAssociacao, chavePrincipal);

            // 3. Criar lista tipada dinamicamente
            var lista = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(tipoItemLista));

            // 4. Carregar cada objeto associado
            foreach (var chave in chavesAssociadas)
            {
                var objetoAssociado = await CarregarObjetoAssociado(tipoItemLista, chave);
                if (objetoAssociado != null)
                {
                    lista.Add(objetoAssociado);
                }
            }

            prop.SetValue(item, lista);
        }

        private async Task<List<object>> ObterChavesAssociadas(AssociacaoBD atributoAssociacao, string chavePrincipal)
        {
            var tipoAssociacao = atributoAssociacao.ObjetoAssociacao;
            var nomeTabelaAssociacao = tipoAssociacao.GetCustomAttribute<TabelaBD>()?.Nome;

            if (string.IsNullOrEmpty(nomeTabelaAssociacao))
                throw new InvalidOperationException($"Tabela não definida para o tipo {tipoAssociacao.Name}");

            // Obter nomes reais das colunas
            var propriedades = tipoAssociacao.GetProperties();
            var nomeColunaEsquerda = ObterNomeColuna(propriedades, atributoAssociacao.PropriedadeEsquerda);
            var nomeColunaDireita = ObterNomeColuna(propriedades, atributoAssociacao.PropriedadeDireita);

            var sql = $"SELECT {nomeColunaDireita} FROM {nomeTabelaAssociacao} WHERE {nomeColunaEsquerda} = @chavePrincipal";

            var chaves = new List<object>();

            using (var connection = new FbConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new FbCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@chavePrincipal", chavePrincipal);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var valor = reader[0];
                            if (valor != DBNull.Value)
                            {
                                chaves.Add(valor);
                            }
                        }
                    }
                }
            }

            return chaves;
        }

        private string ObterNomeColuna(PropertyInfo[] propriedades, string nomePropriedade)
        {
            var propriedade = propriedades.FirstOrDefault(p =>
                p.Name.Equals(nomePropriedade, StringComparison.OrdinalIgnoreCase));

            if (propriedade == null)
                throw new InvalidOperationException($"Propriedade {nomePropriedade} não encontrada");

            return propriedade.GetCustomAttribute<ColunaBD>()?.Nome ?? propriedade.Name;
        }

        private async Task<object> ObterChaveAssociada(AssociacaoBD atributoAssociacao, object valorChavePrincipal)
        {
            // Validações iniciais (mantidas da versão anterior)
            if (atributoAssociacao == null) throw new ArgumentNullException(nameof(atributoAssociacao));
            if (valorChavePrincipal == null) throw new ArgumentNullException(nameof(valorChavePrincipal));

            var tipoAssociacao = atributoAssociacao.ObjetoAssociacao;
            var nomeTabelaAssociacao = tipoAssociacao.GetCustomAttribute<TabelaBD>()?.Nome;

            if (string.IsNullOrEmpty(nomeTabelaAssociacao))
                throw new InvalidOperationException($"Tabela não definida para o tipo {tipoAssociacao.Name}");

            // Método auxiliar para obter nomes de colunas
            string ObterNomeColuna(string nomePropriedade) =>
                tipoAssociacao.GetProperties()
                    .FirstOrDefault(p => p.Name.Equals(nomePropriedade, StringComparison.OrdinalIgnoreCase))
                    ?.GetCustomAttribute<ColunaBD>()?.Nome ?? nomePropriedade;

            var nomeColunaEsquerda = ObterNomeColuna(atributoAssociacao.PropriedadeEsquerda);
            var nomeColunaDireita = ObterNomeColuna(atributoAssociacao.PropriedadeDireita);

            // Consulta SQL
            var sql = $"SELECT {nomeColunaDireita} FROM {nomeTabelaAssociacao} WHERE {nomeColunaEsquerda} = @chavePrincipal";

            using var connection = new FbConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new FbCommand(sql, connection);
            command.Parameters.AddWithValue("@chavePrincipal", valorChavePrincipal);

            return await command.ExecuteScalarAsync();
        }

        private async Task<object> CarregarObjetoAssociado(Type tipoObjetoAssociado, object chave)
        {
            try
            {
                // 1. Validar parâmetros de entrada
                if (tipoObjetoAssociado == null)
                    throw new ArgumentNullException(nameof(tipoObjetoAssociado));

                if (chave == null)
                    throw new ArgumentNullException(nameof(chave));

                // 2. Obter metadados da tabela
                var nomeTabela = tipoObjetoAssociado.GetCustomAttribute<TabelaBD>()?.Nome;
                if (string.IsNullOrEmpty(nomeTabela))
                    throw new InvalidOperationException(
                        $"Atributo [TabelaBD] não encontrado ou inválido para o tipo {tipoObjetoAssociado.Name}");

                // 3. Encontrar a propriedade chave (case-insensitive e com fallback para [ColunaBD])
                var propriedadeChave = tipoObjetoAssociado.GetProperties()
                    .FirstOrDefault(p => p.Name.Equals("Chave", StringComparison.OrdinalIgnoreCase) ||
                                       p.GetCustomAttribute<ColunaBD>()?.Nome?.Equals("chave", StringComparison.OrdinalIgnoreCase) == true)
                    ?? throw new InvalidOperationException(
                        $"Nenhuma propriedade 'Chave' encontrada no tipo {tipoObjetoAssociado.Name}");

                // 4. Obter nome real da coluna no BD (usar [ColunaBD] se existir)
                var nomeColunaChave = propriedadeChave.GetCustomAttribute<ColunaBD>()?.Nome ?? propriedadeChave.Name;

                // 5. Obter todas as colunas mapeadas
                var colunas = tipoObjetoAssociado.GetProperties()
                    .Where(p => p.GetCustomAttribute<ColunaBD>() != null)
                    .Select(p => p.GetCustomAttribute<ColunaBD>().Nome)
                    .ToList();

                if (colunas.Count == 0)
                    throw new InvalidOperationException(
                        $"Nenhuma propriedade com [ColunaBD] encontrada no tipo {tipoObjetoAssociado.Name}");

                // 6. Construir a consulta SQL
                var sql = $"SELECT {string.Join(", ", colunas)} FROM {nomeTabela} WHERE {nomeColunaChave} = @chave";

                // 7. Executar usando o método auxiliar específico (mais seguro que reflection puro)
                var resultado = await ExecutarConsultaAssociacao(sql, tipoObjetoAssociado, chave);

                return resultado?.Count > 0 ? resultado[0] : null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Falha ao carregar objeto associado do tipo {tipoObjetoAssociado?.Name}. " +
                    $"Chave: {chave}. Detalhes: {ex.Message}", ex);
            }
        }

        private async Task<IList> ExecutarConsultaAssociacao(string sql, Type tipoObjeto, object chave)
        {
            try
            {
                var metodo = typeof(DBContexto<T>)
                    .GetMethod(nameof(ExecutarConsultaGenerica), BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.MakeGenericMethod(tipoObjeto);

                if (metodo == null)
                    throw new InvalidOperationException("Método ExecutarConsultaGenerica não encontrado");

                SQL = sql;
                Parametros = new Dictionary<string, object> { ["@chave"] = chave };

                var task = (Task)metodo.Invoke(this, null);
                await task.ConfigureAwait(false);

                // Get the result from the task
                var resultProperty = task.GetType().GetProperty("Result");
                return (IList)resultProperty.GetValue(task);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Falha ao executar consulta para o tipo {tipoObjeto.Name}. SQL: {sql}", ex);
            }
        }

        private Task<List<TAssoc>> ExecutarConsultaGenerica<TAssoc>() where TAssoc : new()
        {
            return ExecutarConsulta<TAssoc>();
        }


        protected Task<List<T>> ExecutarConsulta<T>(string sql, Dictionary<string, object> parametros) where T : new()
        {
            SQL = sql;
            Parametros = parametros;
            return ExecutarConsulta<T>();
        }

        public async Task Adicionar<T>(T entidade) where T : BaseObjetoPersistente
        {
            if (entidade == null) throw new ArgumentNullException(nameof(entidade));

            var tabela = ObterNomeTabela<T>();
            var propriedades = ObterPropriedadesMapeadas<T>();

            var colunas = string.Join(", ", propriedades.Select(p => p.GetCustomAttribute<ColunaBD>().Nome));
            var valores = string.Join(", ", propriedades.Select(p => $"@{p.Name}"));

            var query = $"INSERT INTO {tabela} ({colunas}) VALUES ({valores})";

            await ExecutarComandoComParametros(query, propriedades, entidade);
        }

        public async Task Atualizar<T>(T entidade) where T : BaseObjetoPersistente
        {
            if (entidade == null) throw new ArgumentNullException(nameof(entidade));

            entidade.DataAlteracao = DateTime.UtcNow;

            var tabela = ObterNomeTabela<T>();
            var propriedades = ObterPropriedadesMapeadas<T>();

            var setClause = string.Join(", ", propriedades.Select(p => $"{p.GetCustomAttribute<ColunaBD>().Nome} = @{p.Name}"));
            var query = $"UPDATE {tabela} SET {setClause} WHERE chave = @Chave";

            await ExecutarComandoComParametros(query, propriedades, entidade, true);
        }

        private async Task ExecutarComandoComParametros<T>(string query, IEnumerable<PropertyInfo> propriedades, T entidade, bool incluirChave = false) where T : BaseObjetoPersistente
        {
            using var connection = new FbConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new FbCommand(query, connection);

            foreach (var prop in propriedades)
            {
                var valor = prop.GetValue(entidade) ?? DBNull.Value;

                if (IsEnumeradorSeguro(prop.PropertyType))
                {
                    var valorProp = prop.PropertyType.GetProperty("Valor");
                    if (valorProp != null)
                    {
                        var valorEnumerador = valor != null ? valorProp.GetValue(valor) : null;
                        command.Parameters.AddWithValue($"@{prop.Name}", valorEnumerador ?? DBNull.Value);
                        continue;
                    }
                }

                command.Parameters.AddWithValue($"@{prop.Name}", valor);
            }

            if (incluirChave)
            {
                command.Parameters.AddWithValue("@Chave", entidade.Chave);
            }

            await command.ExecuteNonQueryAsync();
        }       

        public async Task<List<T>> ObterTodos()
        {
            SQL = $"SELECT {ColunasFormatada} FROM {NomeTabela()}";
            return await ExecutarConsulta<T>();
        }

        public async Task<T> ObterPorChave(string chave)
        {
            SQL = $"SELECT {ColunasFormatada} FROM {NomeTabela()} WHERE chave = @chave";
            Parametros.Add("@chave", chave.ToString());

            var resultado = await ExecutarConsulta<T>();
            return resultado.FirstOrDefault() ?? new T();
        }

        private async Task <List<T>> Mapear<T>(DataTable table) where T : new()
        {
            if (table == null) throw new ArgumentNullException(nameof(table));

            var lista = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                var entidade = new T();

                foreach (var prop in typeof(T).GetProperties())
                {
                    var atributoColuna = prop.GetCustomAttribute<ColunaBD>();
                    var atributoColunaObjeto = prop.GetCustomAttribute<ColunaBDObjeto>();

                    if (atributoColuna != null && table.Columns.Contains(atributoColuna.Nome))
                    {
                        var valor = row[atributoColuna.Nome];
                        if (valor != DBNull.Value)
                        {
                            var valorConvertido = ConverterValor(valor, prop.PropertyType);
                            prop.SetValue(entidade, valorConvertido);
                        }
                    }
                    else if (atributoColunaObjeto != null && table.Columns.Contains(atributoColunaObjeto.Nome))
                    {
                        var chaveAssociada = row[atributoColunaObjeto.Nome];
                        if (chaveAssociada != DBNull.Value)
                        {
                            var objetoAssociado = await CarregarObjetoAssociado(prop.PropertyType, chaveAssociada);
                            prop.SetValue(entidade, objetoAssociado);
                        }
                    }
                }

                lista.Add(entidade);
            }

            return lista;
        }

        public string NomeTabela() => ObterNomeTabela<T>();

        private static string ObterNomeTabela<TType>()
        {
            var nomeTabela = typeof(TType).GetCustomAttribute<TabelaBD>()?.Nome;
            return string.IsNullOrEmpty(nomeTabela)
                ? throw new InvalidOperationException($"A classe {typeof(TType).Name} não possui o atributo [TabelaBD].")
                : nomeTabela;
        }

        private static IEnumerable<PropertyInfo> ObterPropriedadesMapeadas<TType>()
        {
            return typeof(TType).GetProperties()
                .Where(p => p.GetCustomAttribute<ColunaBD>() != null)
                .ToList();
        }

        private static object ConverterValor(object valor, Type targetType)
        {
            // Tratamento para Guid
            if (targetType == typeof(Guid))
                return Guid.Parse(valor.ToString());

            // Tratamento para enums
            if (targetType.IsEnum)
                return Enum.ToObject(targetType, valor);

            // Tratamento para enumeradores seguros
            if (IsEnumeradorSeguro(targetType))
            {
                var tipoValor = targetType.BaseType?.GetGenericArguments()[1];
                var valorConvertido = Convert.ChangeType(valor, tipoValor);

                var metodoObterPorValor = targetType.GetMethod("ObterPorValor",
                    BindingFlags.Public | BindingFlags.Static,
                    null,
                    new[] { tipoValor },
                    null);

                if (metodoObterPorValor != null)
                {
                    return metodoObterPorValor.Invoke(null, new[] { valorConvertido });
                }
            }

            // Tratamento específico para DateOnly
            if (targetType == typeof(DateOnly) && valor is DateTime dateTime)
            {
                return DateOnly.FromDateTime(dateTime);
            }

            // Tratamento para DateOnly?
            if (Nullable.GetUnderlyingType(targetType) == typeof(DateOnly) && valor is DateTime nullableDateTime)
            {
                return DateOnly.FromDateTime(nullableDateTime);
            }

            // Tratamento para tipos nullable
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                if (underlyingType == typeof(DateOnly) && valor is DateTime dt)
                {
                    return DateOnly.FromDateTime(dt);
                }
                return Convert.ChangeType(valor, underlyingType);
            }

            return Convert.ChangeType(valor, targetType);
        }

        private static bool IsEnumeradorSeguro(Type type)
        {
            return type?.BaseType is { IsGenericType: true } baseType &&
             baseType.GetGenericTypeDefinition() == typeof(EnumeradorSeguro<,>);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Liberar recursos gerenciados aqui
            }

            _disposed = true;
        }

        ~DBContexto()
        {
            Dispose(false);
        }
    }
}