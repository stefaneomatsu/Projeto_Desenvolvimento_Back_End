using Microsoft.AspNetCore.Mvc;
using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Servicos;
using System.Linq;

namespace SGHSS_Uninter.Api.Controllers
{
    public class PacienteController : BaseController
    {
        private readonly IPacienteServico _pacienteServico;

        public PacienteController(
            IConfiguration configuration,
            ILogger<UsuarioController> logger,
            IPacienteServico pacienteServico,
            SessaoDAO sessaoDAO)
            : base(configuration, logger, sessaoDAO)
        {
            _pacienteServico = pacienteServico;
        }

        [HttpPost("CriarPaciente")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO)]
        public async Task<IActionResult> CriarPaciente(string token, [FromBody] PacienteNovoDTO pacienteNovo)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos fornecidos");
                return BadRequest(ModelState);
            }

            var resultado = await _pacienteServico.CriarPlanoSaudeAsync(pacienteNovo);

            return TratarResultado(resultado);
        }

        [HttpGet("ObterPaciente")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO,
           EnumPerfilAcesso.MEDICO,
           EnumPerfilAcesso.ENFERMEIRO)]
        public async Task<IActionResult> ObterPaciente(string token, string cpf)
        {
            var resultado = await _pacienteServico.ObterPaciente(cpf);

            return TratarResultado(resultado, paciente => new PacienteDTO(resultado.Dados));
        }

        [HttpGet("ObterPacientes")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO)]
        public async Task<IActionResult> ObterPacientes(string token)
        {
            var resultado = await _pacienteServico.ObterPacientes();

            return TratarResultado(resultado, pacientes => resultado.Dados.Select(x => new PacienteDTO(x)));
        }
    }
}
