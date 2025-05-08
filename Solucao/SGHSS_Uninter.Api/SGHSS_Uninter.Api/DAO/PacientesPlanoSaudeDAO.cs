using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.DAO
{
    public class PacientesPlanoSaudeDAO : BaseDAO<PacientesPlanoSaude>
    {
        public PacientesPlanoSaudeDAO(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<ResultadoOperacao<bool>> VerificarAdicionarVinculo(PacientesPlanoSaude pacientesPlanoSaude)
        {
            SQL = $"SELECT chave FROM {NomeTabela()} WHERE " +
                 $"paciente = @paciente " +
                 $"AND plano_saude = @plano_saude " +
                 $"AND numero_carteira = @numero_carteira";

            Parametros = new Dictionary<string, object>()
            {
                ["@paciente"] = pacientesPlanoSaude.ChavePaciente,
                ["@plano_saude"] = pacientesPlanoSaude.ChavePlanoSaude,
                ["@numero_carteira"] = pacientesPlanoSaude.NumeroCarteira
            };

            var retorno = await ExecutarConsulta();

            if (retorno != null &&
                retorno.Count > 0)
            {
                return ResultadoOperacao<bool>.CriarFalha("Paciente já vinculado ao plano");
            }

            return ResultadoOperacao<bool>.CriarSucesso();
        }
    }
}
