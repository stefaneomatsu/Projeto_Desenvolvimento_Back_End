using Microsoft.AspNetCore.Mvc;
using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models;

namespace SGHSS_Uninter.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> _logger;
        protected readonly IConfiguration _configuration;
        protected readonly SessaoDAO _sessaoDAO;

        public BaseController(
            IConfiguration configuration, 
            ILogger<BaseController> logger,
            SessaoDAO sessaoDAO)
        {
            _logger = logger;
            _configuration = configuration;
            _sessaoDAO = sessaoDAO;
        }

        protected IActionResult TratarResultado<T>(ResultadoOperacao<T> resultado,
                                                 Func<T, object> transformarDados = null)
        {
            if (resultado.Sucesso)
            {
                var dadosRetorno = transformarDados != null
                    ? transformarDados(resultado.Dados)
                    : resultado.Dados;

                return Ok(dadosRetorno);
            }

            _logger.LogWarning(resultado.Mensagem);

            // Padroniza o formato de erro
            return BadRequest(new
            {
                Sucesso = false,
                Mensagem = resultado.Mensagem
            });
        }
    }
}
