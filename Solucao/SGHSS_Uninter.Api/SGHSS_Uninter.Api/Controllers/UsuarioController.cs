using Microsoft.AspNetCore.Mvc;
using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Models.Persistente;
using SGHSS_Uninter.Api.Servicos;
using SGHSS_Uninter.Api.Utilitarios;

namespace SGHSS_Uninter.Api.Controllers
{
    public class UsuarioController : BaseController
    {
        private readonly IUsuarioServico _usuarioServico;

        public UsuarioController(
            IConfiguration configuration, 
            ILogger<UsuarioController> logger,
            IUsuarioServico usuarioServico,
            SessaoDAO sessaoDAO) 
            : base(configuration, logger, sessaoDAO)
        {
            _usuarioServico = usuarioServico;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string usuario, string senha)
        {
            var resultado = await _usuarioServico.LoginAsync(
                usuario,
                senha);

            return TratarResultado(resultado, sessao => new SessaoDTO(resultado.Dados));
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout(string token)
        {
            var resultado = await _usuarioServico.LogoutAsync(token);

            return TratarResultado(resultado, sessao => resultado);
        }

        [HttpGet("InformacoesSessao")]
        public async Task<IActionResult> InformacoesSessao(string token)
        {
            var resultado = await _usuarioServico.ObterInformacoesSessaoAsync(token);

            return TratarResultado(resultado, sessao => new SessaoDTO(resultado.Dados));
        }

        [HttpPost("CriarUsuario")]
        [AutorizacaoPorPermissao(
            EnumPerfilAcesso.ADMINISTRADOR,
            EnumPerfilAcesso.RECEPCAO)]
        public async Task<IActionResult> CriarUsuario(string token, [FromBody] UsuarioNovoDTO usuarioNovo)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos fornecidos");
                return BadRequest(ModelState);
            }

            var resultado = await _usuarioServico.CriarUsuarioAsync(usuarioNovo);

            return TratarResultado(resultado);
        }
    }
}
