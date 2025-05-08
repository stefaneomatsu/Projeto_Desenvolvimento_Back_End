using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.DAO
{
    public class LaboratorioDAO : BaseDAO<Laboratorio>
    {

        public LaboratorioDAO(IConfiguration configuration)
        : base(configuration)
        {

        }

        public async Task<ResultadoOperacao<Laboratorio>> ObterLaboratorioPorCNPJ(string cnpj)
        {
            SQL = $"SELECT {ColunasFormatada} FROM {NomeTabela()} WHERE " +
                  $"cnpj = @cnpj";

            Parametros = new Dictionary<string, object>()
            {
                ["@cnpj"] = cnpj,
            };

            var retorno = await ExecutarConsulta();

            if (retorno != null &&
                retorno.Count > 0)
            {
                return ResultadoOperacao<Laboratorio>.CriarSucesso(retorno[0]);
            }

            return ResultadoOperacao<Laboratorio>.CriarFalha("Profissional não encontrado");
        }
    }
}
