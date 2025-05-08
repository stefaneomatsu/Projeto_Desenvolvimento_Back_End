using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.DAO
{
    public class ExameDAO : BaseDAO<Exame>
    {
        public ExameDAO(IConfiguration configuration)
      : base(configuration)
        {

        }

        public async Task<ResultadoOperacao<List<Exame>>> ObterExamesPorProfissional(Profissional profissional)
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
                return ResultadoOperacao<List<Exame>>.CriarSucesso(retorno);
            }

            return ResultadoOperacao<List<Exame>>.CriarFalha("Exames não encontrado");
        }

        public async Task<ResultadoOperacao<List<Exame>>> ObterExamesPorPaciente(Paciente paciente)
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
                return ResultadoOperacao<List<Exame>>.CriarSucesso(retorno);
            }

            return ResultadoOperacao<List<Exame>>.CriarFalha("Exames não encontrados");
        }
    }
}
