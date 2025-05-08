using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.Servicos
{
    public interface IProfissionalServico
    {
        Task<ResultadoOperacao<bool>> CriarProfissionalAsync(ProfissionalNovoDTO profissionalNovo);
    }

    public class ProfissionalServico : BaseServico, IProfissionalServico
    {
        private readonly ProfissionalDAO _profissionalDAO;

        public ProfissionalServico(
            IConfiguration configuration,
            ILogger<UsuarioServico> logger,
            ProfissionalDAO profissionalDAO) : base(configuration, logger)
        {
            _profissionalDAO = profissionalDAO;
        }

        public async Task<ResultadoOperacao<bool>> CriarProfissionalAsync(ProfissionalNovoDTO profissionalNovo)
        {
            try
            {
                var profissional = new Profissional(profissionalNovo);
                var resultado = await _profissionalDAO.VerificarInserirProfissional(profissional);
                
                if (!resultado.Sucesso)
                {
                    return resultado;
                }

                await _profissionalDAO.AdicionarProfissional(profissional);

                return ResultadoOperacao<bool>.CriarSucesso();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao criar Profissional");
                return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha("Erro ao criar Profissional"));
            }
        }
    }
}
