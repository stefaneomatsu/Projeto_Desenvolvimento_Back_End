using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.DAO
{
    public class UnidadeDAO : BaseDAO<UnidadeHospitalar>
    {
        public UnidadeDAO(IConfiguration configuration)
        : base(configuration)
        {

        }

        public async Task<ResultadoOperacao<UnidadeHospitalar>> ObtenhaUnidade(int codigo)
        {
            SQL = $"SELECT {ColunasFormatada} FROM {NomeTabela()} WHERE codigo = @codigo";

            Parametros = new Dictionary<string, object>()
            {
                ["@codigo"] = codigo
            };

            var resultados = await ExecutarConsulta();

            if (resultados != null && 
                resultados.Count > 0)
            {
                return ResultadoOperacao<UnidadeHospitalar>.CriarSucesso(resultados[0]);
            }

            return ResultadoOperacao<UnidadeHospitalar>.CriarFalha("Unidade não encontrada");
        }

        public async Task<bool> VerificarExisteUnidadeCodigo(int codigo)
        {
            SQL = $"SELECT chave FROM {NomeTabela()} WHERE " +
                  $"codigo= @codigo";

            Parametros = new Dictionary<string, object>()
            {
                ["@codigo"] = codigo,
            };

            var retorno = await ExecutarConsulta();

            if (retorno != null &&
                retorno.Count > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> VerificarExisteUnidadeCnpj(string cnpj)
        {
            SQL = $"SELECT chave FROM {NomeTabela()} WHERE " +
                  $"cnpj= @cnpj";

            Parametros = new Dictionary<string, object>()
            {
                ["@cnpj"] = cnpj,
            };

            var retorno = await ExecutarConsulta();

            if (retorno != null &&
                retorno.Count > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<ResultadoOperacao<bool>> VerificarInserirUnidade(UnidadeHospitalar unidade)
        {
            if (await VerificarExisteUnidadeCodigo(unidade.Codigo))
            {
                return ResultadoOperacao<bool>.CriarFalha("Código já cadastrado");
            }
            if (await VerificarExisteUnidadeCnpj(unidade.CNPJ))
            {
                return ResultadoOperacao<bool>.CriarFalha("Cnpj já cadastrado");
            }
            return ResultadoOperacao<bool>.CriarSucesso();
        }
    }
}
