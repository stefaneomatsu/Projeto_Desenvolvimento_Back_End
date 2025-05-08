using SGHSS_Uninter.Api.DAO;

namespace SGHSS_Uninter.Api.Servicos
{
    public class BaseServico
    {
        protected readonly IConfiguration _configuration;
        protected readonly ILogger<BaseServico> _logger;

        public BaseServico(
            IConfiguration configuration,
            ILogger<BaseServico> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }


    }
}
