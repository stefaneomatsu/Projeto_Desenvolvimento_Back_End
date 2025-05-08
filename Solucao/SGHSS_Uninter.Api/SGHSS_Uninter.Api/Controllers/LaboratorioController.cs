using Microsoft.AspNetCore.Mvc;
using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Servicos;

namespace SGHSS_Uninter.Api.Controllers
{
    public class LaboratorioController : BaseController
    {
        private readonly ILaboratorioServico _laboratorioServico;

        public LaboratorioController(
            IConfiguration configuration,
            ILogger<UsuarioController> logger,
            ILaboratorioServico laboratorioServico,
            SessaoDAO sessaoDAO)
            : base(configuration, logger, sessaoDAO)
        {
            _laboratorioServico = laboratorioServico;
        }

        [HttpPost("CriarLaboratorio")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO)]
        public async Task<IActionResult> CriarLaboratorio(string token, [FromBody] LaboratorioNovoDTO laboratorioNovo)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos fornecidos");
                return BadRequest(ModelState);
            }

            var resultado = await _laboratorioServico.CriarLaboratorioAsync(laboratorioNovo);

            return TratarResultado(resultado);
        }

        [HttpGet("ObterLaboratorios")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO)]
        public async Task<IActionResult> ObterLaboratorios(string token)
        {
            var resultado = await _laboratorioServico.ObterLaboratorios();

            return TratarResultado(resultado, laboratorios => resultado.Dados.Select(x => new LaboratorioDTO(x)));
        }
    }
}
