using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.Servicos
{
    public interface ILaboratorioServico
    {
        Task<ResultadoOperacao<bool>> CriarLaboratorioAsync(LaboratorioNovoDTO laboratorioNovo);
        Task<ResultadoOperacao<List<Laboratorio>>> ObterLaboratorios();
    }

    public class LaboratorioServico : BaseServico, ILaboratorioServico
    {
        private readonly LaboratorioDAO _laboratorioDAO;

        public LaboratorioServico(
            IConfiguration configuration,
            ILogger<UsuarioServico> logger,
            LaboratorioDAO laboratorioDAO) : base(configuration, logger)
        {
            _laboratorioDAO = laboratorioDAO;
        }

        public async Task<ResultadoOperacao<bool>> CriarLaboratorioAsync(LaboratorioNovoDTO laboratorioNovo)
        {
            try
            {
                var laboratorio = new Laboratorio(laboratorioNovo);

                await _laboratorioDAO.Adicionar(laboratorio);

                return await Task.FromResult(ResultadoOperacao<bool>.CriarSucesso());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao criar Laboratório");
                return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha("Erro ao criar Laboratório"));
            }
        }

        public async Task<ResultadoOperacao<List<Laboratorio>>> ObterLaboratorios()
        {
            try
            {
                var laboratorios = await _laboratorioDAO.ObterTodos();

                if (laboratorios != null &&
                    laboratorios.Count > 0)
                {
                    return await Task.FromResult(ResultadoOperacao<List<Laboratorio>>.CriarSucesso(laboratorios));
                }

                return await Task.FromResult(ResultadoOperacao<List<Laboratorio>>.CriarFalha("Nenhum laboratório encontrado"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao criar Laboratório");
                return await Task.FromResult(ResultadoOperacao<List<Laboratorio>>.CriarFalha("Erro ao obter Laboratórios"));
            }
        }
    }
}
