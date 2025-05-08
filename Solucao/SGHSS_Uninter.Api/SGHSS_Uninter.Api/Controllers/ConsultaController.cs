using Microsoft.AspNetCore.Mvc;
using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Servicos;
using System.Linq;

namespace SGHSS_Uninter.Api.Controllers
{
    public class ConsultaController : BaseController
    {
        private readonly IConsultaServico _consultaServico;

        public ConsultaController(
            IConfiguration configuration,
            ILogger<UsuarioController> logger,
            IConsultaServico consultaServico,
            SessaoDAO sessaoDAO)
            : base(configuration, logger, sessaoDAO)
        {
            _consultaServico = consultaServico;
        }

        [HttpPost("CriarConsulta")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO)]
        public async Task<IActionResult> CriarConsulta(string token, [FromBody] ConsultaNovoDTO consultaNovo)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos fornecidos");
                return BadRequest(ModelState);
            }

            var resultado = await _consultaServico.CriarConsultaAsync(consultaNovo);

            return TratarResultado(resultado);
        }

        [HttpGet("ObterConsultasPorProfissional")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO,
           EnumPerfilAcesso.MEDICO,
           EnumPerfilAcesso.ENFERMEIRO)]
        public async Task<IActionResult> ObterConsultasPorProfissionalCPF(string token, string cpf)
        {
            var resultado = await _consultaServico.ObterConsultasPorCPFProfissionalAsync(cpf);

            return TratarResultado(resultado, consultas => resultado.Dados.Select(x => new ConsultaDTO(x)));
        }

        [HttpGet("ObterConsultasPorPaciente")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.ADMINISTRADOR,
           EnumPerfilAcesso.RECEPCAO,
           EnumPerfilAcesso.MEDICO,
           EnumPerfilAcesso.ENFERMEIRO)]
        public async Task<IActionResult> ObterConsultasPorPacienteCPF(string token, string cpf)
        {
            var resultado = await _consultaServico.ObterConsultasPorCPFPacienteAsync(cpf);

            return TratarResultado(resultado, consultas => resultado.Dados.Select(x => new ConsultaDTO(x)));
        }

        [HttpGet("ObterConsultasPorPacienteToken")]
        [AutorizacaoPorPermissao(
           EnumPerfilAcesso.PACIENTE)]
        public async Task<IActionResult> ObterConsultasPorPacienteToken(string token)
        {
            var resultado = await _consultaServico.ObterConsultasPorTokenAsync(token);

            return TratarResultado(resultado, consultas => resultado.Dados.Select(x => new ConsultaDTO(x)));
        }

    }
}
