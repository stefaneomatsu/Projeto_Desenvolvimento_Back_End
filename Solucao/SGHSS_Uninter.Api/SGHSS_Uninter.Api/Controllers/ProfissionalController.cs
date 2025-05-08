using Microsoft.AspNetCore.Mvc;
using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Servicos;

namespace SGHSS_Uninter.Api.Controllers
{
    public class ProfissionalController : BaseController
    {
        private readonly IProfissionalServico _profissionalServico;

        public ProfissionalController(
            IConfiguration configuration,
            ILogger<UsuarioController> logger,
            IProfissionalServico profissionalServico,
            SessaoDAO sessaoDAO)
            : base(configuration, logger, sessaoDAO)
        {
            _profissionalServico = profissionalServico;
        }

        [HttpPost("CriarProfissional")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO)]
        public async Task<IActionResult> CriarUsuario(string token, [FromBody] ProfissionalNovoDTO profissionalNovo)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos fornecidos");
                return BadRequest(ModelState);
            }

            var resultado = await _profissionalServico.CriarProfissionalAsync(profissionalNovo);

            return TratarResultado(resultado);
        }
    }
}
