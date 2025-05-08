using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.DAO
{
    public class ConsultaDAO : BaseDAO<Consulta>
    {
        public ConsultaDAO(IConfiguration configuration)
        : base(configuration)
        {

        }

        public async Task<ResultadoOperacao<List<Consulta>>> ObterConsultasPorProfissional(Profissional profissional)
        {
            SQL = $"SELECT {ColunasFormatada} FROM {NomeTabela()} WHERE " +
                  $"profissional = @chaveProfissional";

            Parametros = new Dictionary<string, object>()
            {
                ["@chaveProfissional"] = profissional.Chave,
            };

            var retorno = await ExecutarConsulta();

            if (retorno != null &&
                retorno.Count > 0)
            {
                return ResultadoOperacao<List<Consulta>>.CriarSucesso(retorno);
            }

            return ResultadoOperacao<List<Consulta>>.CriarFalha("Consultas não encontrado");
        }

        public async Task<ResultadoOperacao<List<Consulta>>> ObterConsultasPorPaciente(Paciente paciente)
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
                return ResultadoOperacao<List<Consulta>>.CriarSucesso(retorno);
            }

            return ResultadoOperacao<List<Consulta>>.CriarFalha("Consultas não encontradas");
        }
    }
}
