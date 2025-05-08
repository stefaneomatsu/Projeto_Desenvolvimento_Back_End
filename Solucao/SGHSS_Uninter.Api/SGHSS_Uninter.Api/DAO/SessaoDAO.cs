using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.DAO
{
    public class SessaoDAO : BaseDAO<Sessao>
    {
        public SessaoDAO(IConfiguration configuration)
        : base(configuration)
        {

        }

        public async Task IncluirNovaSessao(Sessao sessao)
        {
            SQL = $"SELECT {ColunasFormatada} FROM {NomeTabela()} WHERE usuario = @chaveUsuario";

            Parametros = new Dictionary<string, object>()
            {
                ["@chaveUsuario"] = sessao.Usuario.Chave
            };

            var resultados = await ExecutarConsulta();

            if (resultados != null &&
                resultados.Count > 0)
            {
                SQL =   $"UPDATE {NomeTabela()} SET " +
                        $"status = @status," +
                        $"data_saida = @dataSaida" +
                        $" WHERE usuario = @chaveUsuario";

                Parametros = new Dictionary<string, object>()
                {
                    ["@status"] = (int)EnumStatusRegistro.INATIVO,
                    ["@chaveUsuario"] = sessao.Usuario.Chave,
                    ["@dataSaida"] = DateTime.UtcNow
                };

                await ExecutarComando();
            }           

            await Adicionar(sessao);
        }

        public async Task <Sessao> ObterSessao(string token)
        {
            SQL = $"SELECT {ColunasFormatada} FROM {NomeTabela()} WHERE token = @token AND status = @status";

            Parametros = new Dictionary<string, object>()
            {
                ["@token"] = token,
                ["@status"] = (int)EnumStatusRegistro.ATIVO
            };

            var retornoConsulta = await ExecutarConsulta();

            if (retornoConsulta != null &&
                retornoConsulta.Count > 0)
            {
                return retornoConsulta[0];
            }

            return null;
        }

        public async Task SairSessao(Sessao sessao)
        {
            SQL = $"UPDATE {NomeTabela()} SET " +
                       $"status = @status," +
                       $"data_saida = @dataSaida" +
                       $" WHERE chave = @chave";

            Parametros = new Dictionary<string, object>()
            {
                ["@status"] = (int)EnumStatusRegistro.INATIVO,
                ["@dataSaida"] = DateTime.UtcNow,
                ["@chave"] = sessao.Chave
            };

            await ExecutarComando();
        }
    }
}
