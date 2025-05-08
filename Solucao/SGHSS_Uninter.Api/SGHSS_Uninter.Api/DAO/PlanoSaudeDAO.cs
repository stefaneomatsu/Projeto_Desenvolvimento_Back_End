using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.DAO
{
    public class PlanoSaudeDAO : BaseDAO<PlanoSaude>
    {
        public PlanoSaudeDAO(IConfiguration configuration)
        : base(configuration)
        {

        }

        public async Task<ResultadoOperacao<bool>> VerificarInserirPlanoSaude(PlanoSaude planoSaude)
        {
            var resultadoANS = await VerificarExisteRegistroAns(planoSaude.RegistroANS);
            if (resultadoANS.Sucesso)
            {
                return ResultadoOperacao<bool>.CriarFalha("Plano de saúde já cadastrado");
            }

            return ResultadoOperacao<bool>.CriarSucesso();
        }

        public async Task<ResultadoOperacao<bool>> VerificarExisteRegistroAns(string registro_ans)
        {
            SQL = $"SELECT chave FROM {NomeTabela()} WHERE " +
                  $"registro_ans = @registro_ans";

            Parametros = new Dictionary<string, object>()
            {
                ["@registro_ans"] = registro_ans,
            };

            var retorno = await ExecutarConsulta();

            if (retorno != null &&
                retorno.Count > 0)
            {
                return ResultadoOperacao<bool>.CriarSucesso();
            }

            return ResultadoOperacao<bool>.CriarFalha();
        }

        public async Task<ResultadoOperacao<PlanoSaude>> ObterPlanoRegistroAns(string registro_ans)
        {
            SQL = $"SELECT {ColunasFormatada} FROM {NomeTabela()} WHERE " +
                  $"registro_ans = @registro_ans";

            Parametros = new Dictionary<string, object>()
            {
                ["@registro_ans"] = registro_ans,
            };

            var retorno = await ExecutarConsulta();

            if (retorno != null &&
                retorno.Count > 0)
            {
                return ResultadoOperacao<PlanoSaude>.CriarSucesso(retorno[0]);
            }

            return ResultadoOperacao<PlanoSaude>.CriarFalha("Plano não encontrado");
        }
    }
}
