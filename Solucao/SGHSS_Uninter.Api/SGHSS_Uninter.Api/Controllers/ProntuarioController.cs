using Microsoft.AspNetCore.Mvc;
using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Servicos;

namespace SGHSS_Uninter.Api.Controllers
{
    public class ProntuarioController : BaseController
    {
        private readonly IProntuarioServico _prontuarioServico;

        public ProntuarioController(
            IConfiguration configuration,
            ILogger<UsuarioController> logger,
            IProntuarioServico prontuarioServico,
            SessaoDAO sessaoDAO)
            : base(configuration, logger, sessaoDAO)
        {
            _prontuarioServico = prontuarioServico;
        }

        [HttpPost("CriarProntuario")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO)]
        public async Task<IActionResult> CriarProntuario(string token, [FromBody] ProntuarioNovoDTO prontuarioNovo)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos fornecidos");
                return BadRequest(ModelState);
            }

            var resultado = await _prontuarioServico.CriarProntuarioAsync(prontuarioNovo);

            return TratarResultado(resultado);
        }


        [HttpPost("VincularProntuarioEntrada")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO)]
        public async Task<IActionResult> VincularProntuarioEntrada(string token, [FromBody] ProntuarioEntradaNovoDTO prontuarioEntradaNovo)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos fornecidos");
                return BadRequest(ModelState);
            }

            var resultado = await _prontuarioServico.VincularProntuarioEntradaAsync(prontuarioEntradaNovo);

            return TratarResultado(resultado);
        }

        [HttpGet("ObterProntuarioPaciente")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO,
           EnumPerfilAcesso.MEDICO,
           EnumPerfilAcesso.ENFERMEIRO)]
        public async Task<IActionResult> ObterProntuarioPaciente(string token, string cpf)
        {
            var resultado = await _prontuarioServico.ObterProntuarioPacienteAsync(cpf);

            return TratarResultado(resultado, prontuario => new ProntuarioDTO(resultado.Dados));
        }

    }
}
