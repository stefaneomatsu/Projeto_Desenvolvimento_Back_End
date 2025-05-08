using SGHSS_Uninter.Api.Models.Persistente;
using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Models.DTO;

namespace SGHSS_Uninter.Api.Servicos
{
    public interface IUnidadeServico
    {
        Task<ResultadoOperacao<UnidadeHospitalar>> ObterUnidadeAsync(int codigo);
        Task<ResultadoOperacao<List<UnidadeHospitalar>>> ObterUnidadesAsync();
        Task<ResultadoOperacao<bool>> CriarUnidade(UnidadeNovoDTO unidadeNovo);
    }

    public class UnidadeServico : BaseServico, IUnidadeServico
    {

        private readonly UnidadeDAO _unidadeDAO;

        public UnidadeServico(
            IConfiguration configuration,
            ILogger<UsuarioServico> logger,
            UnidadeDAO unidadeDAO) : base(configuration, logger)
        {
            _unidadeDAO = unidadeDAO;
        }

        public async Task<ResultadoOperacao<bool>> CriarUnidade(UnidadeNovoDTO unidadeNovo)
        {
            var unidade = new UnidadeHospitalar(unidadeNovo);
            var resultado = await _unidadeDAO.VerificarInserirUnidade(unidade);

            if (!resultado.Sucesso)
            {
                return resultado;
            }

            await _unidadeDAO.Adicionar(unidade);

            return ResultadoOperacao<bool>.CriarSucesso();
        }

        public async Task<ResultadoOperacao<UnidadeHospitalar>> ObterUnidadeAsync(int codigo)
        {
            try
            {
                var unidade = await _unidadeDAO.ObtenhaUnidade(codigo);

                return await Task.FromResult(unidade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao obter unidade");
                return await Task.FromResult(ResultadoOperacao<UnidadeHospitalar>.CriarFalha($"Erro ao encontrar unidade com código {codigo}"));
            }
        }

        public async Task<ResultadoOperacao<List<UnidadeHospitalar>>> ObterUnidadesAsync()
        {
            try
            {
                var unidades = await _unidadeDAO.ObterTodos();

                return await Task.FromResult(ResultadoOperacao<List<UnidadeHospitalar>>.CriarSucesso(unidades));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao obter unidade");
                return await Task.FromResult(ResultadoOperacao<List<UnidadeHospitalar>>.CriarFalha("Erro ao encontrar unidade"));
            }
        }
    }
}
