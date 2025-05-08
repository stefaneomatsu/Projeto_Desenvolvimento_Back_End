using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.DAO
{
    public class ProntuarioDAO : BaseDAO<Prontuario>
    {
        public ProntuarioDAO(IConfiguration configuration)
        : base(configuration)
        {

        }

        public async Task<ResultadoOperacao<Prontuario>> ObterProntuarioPorPaciente(Paciente paciente)
        {
            SQL = $"SELECT {ColunasFormatada} FROM {NomeTabela()} WHERE " +
                  $"paciente = @chavePaciente";

            Parametros = new Dictionary<string, object>()
            {
                ["@chavePaciente"] = paciente.Chave,
            };

            var retorno = await ExecutarConsulta();

            if (retorno != null &&
                retorno.Count > 0)
            {
                return ResultadoOperacao<Prontuario>.CriarSucesso(retorno[0]);
            }

            return ResultadoOperacao<Prontuario>.CriarFalha("Prontuário não encontrado");
        }
    }
}
