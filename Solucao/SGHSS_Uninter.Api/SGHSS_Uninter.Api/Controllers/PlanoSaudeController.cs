using Microsoft.AspNetCore.Mvc;
using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Servicos;
using System.Runtime.CompilerServices;

namespace SGHSS_Uninter.Api.Controllers
{
    public class PlanoSaudeController : BaseController
    {
        private readonly IPlanoSaudeServico _planoSaudeServico;

        public PlanoSaudeController(
            IConfiguration configuration,
            ILogger<UsuarioController> logger,
            IPlanoSaudeServico planoSaudeServico,
            SessaoDAO sessaoDAO)
            : base(configuration, logger, sessaoDAO)
        {
            _planoSaudeServico = planoSaudeServico;
        }

        [HttpPost("CriarPlanoSaude")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO)]
        public async Task<IActionResult> CriarPlanoSaude(string token, [FromBody] PlanoSaudeNovoDTO planoSaudeNovo)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos fornecidos");
                return BadRequest(ModelState);
            }

            var resultado = await _planoSaudeServico.CriarPlanoSaudeAsync(planoSaudeNovo);

            return TratarResultado(resultado);
        }


        [HttpPost("VincularPacientePlano")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO)]
        public async Task<IActionResult> VincularPacientePlano(string token, [FromBody] PacientePlanoSaudeNovoDTO pacientePlanoSaudeNovo)
        {
            var resultado = await _planoSaudeServico.VincularPacientePlano(pacientePlanoSaudeNovo);

            return TratarResultado(resultado);
        }
    }
}
