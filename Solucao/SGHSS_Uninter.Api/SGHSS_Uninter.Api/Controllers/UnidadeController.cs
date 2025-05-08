using Microsoft.AspNetCore.Mvc;
using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Servicos;

namespace SGHSS_Uninter.Api.Controllers
{
    public class UnidadeController : BaseController
    {
        private readonly IUnidadeServico _unidadeServico;

        public UnidadeController(
            IConfiguration configuration,
            ILogger<UsuarioController> logger,
            IUnidadeServico unidadeServico,
            SessaoDAO sessaoDAO)
            : base(configuration, logger, sessaoDAO)
        {
            _unidadeServico = unidadeServico;
        }

        [HttpGet("ObterUnidade")]
        [AutorizacaoPorPermissao(
          EnumPerfilAcesso.ADMINISTRADOR,
          EnumPerfilAcesso.RECEPCAO)]
        public async Task<IActionResult> ObterUnidade(string token, int codigo)
        {            
            var resultado = await _unidadeServico.ObterUnidadeAsync(codigo);

            return TratarResultado(resultado, unidade => new UnidadeDTO(resultado.Dados));
        }

        [HttpGet("ObterUnidades")]
        [AutorizacaoPorPermissao(
          EnumPerfilAcesso.ADMINISTRADOR,
          EnumPerfilAcesso.RECEPCAO)]
        public async Task<IActionResult> ObterUnidades(string token)
        {
            var resultado = await _unidadeServico.ObterUnidadesAsync();

            return TratarResultado(resultado, unidades => resultado.Dados.Select(x => new UnidadeDTO(x)));
        }

        [HttpPost("CriarUnidade")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO)]
        public async Task<IActionResult> CriarUnidade(string token, [FromBody] UnidadeNovoDTO unidadeNovo)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos fornecidos");
                return BadRequest(ModelState);
            }

            var resultado = await _unidadeServico.CriarUnidade(unidadeNovo);

            return TratarResultado(resultado);
        }
    }
}
