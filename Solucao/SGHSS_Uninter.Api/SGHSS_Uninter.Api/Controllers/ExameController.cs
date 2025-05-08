using Microsoft.AspNetCore.Mvc;
using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Servicos;

namespace SGHSS_Uninter.Api.Controllers
{
    public class ExameController : BaseController
    {
        private readonly IExameServico _exameServico;

        public ExameController(
            IConfiguration configuration,
            ILogger<UsuarioController> logger,
            IExameServico exameServico,
            SessaoDAO sessaoDAO)
            : base(configuration, logger, sessaoDAO)
        {
            _exameServico = exameServico;
        }

        [HttpPost("CriarExame")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO)]
        public async Task<IActionResult> CriarExame(string token, [FromBody] ExameNovoDTO exameNovo)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos fornecidos");
                return BadRequest(ModelState);
            }

            var resultado = await _exameServico.CriarExameAsync(exameNovo);

            return TratarResultado(resultado);
        }

        [HttpGet("ObterExamesPorProfissional")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO,
           EnumPerfilAcesso.MEDICO,
           EnumPerfilAcesso.ENFERMEIRO)]
        public async Task<IActionResult> ObterExamesPorProfissionalCPF(string token, string cpf)
        {
            var resultado = await _exameServico.ObterExamesPorCPFProfissionalAsync(cpf);

            return TratarResultado(resultado, exames => resultado.Dados.Select(x => new ExameDTO(x)));
        }

        [HttpGet("ObterExamesPorPaciente")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO,
           EnumPerfilAcesso.MEDICO,
           EnumPerfilAcesso.ENFERMEIRO)]
        public async Task<IActionResult> ObterExamesPorPacienteCPF(string token, string cpf)
        {
            var resultado = await _exameServico.ObterExamesPorCPFPacienteAsync(cpf);

            return TratarResultado(resultado, exames => resultado.Dados.Select(x => new ExameDTO(x)));
        }

        [HttpGet("ObterExamesPorPacienteToken")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.PACIENTE)]
        public async Task<IActionResult> ObterExamesPorPacienteToken(string token)
        {
            var resultado = await _exameServico.ObterExamesPorTokenAsync(token);

            return TratarResultado(resultado, exames => resultado.Dados.Select(x => new ExameDTO(x)));
        }
    }
}
