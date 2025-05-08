using Microsoft.AspNetCore.Identity;
using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Models.Persistente;
using SGHSS_Uninter.Api.Utilitarios;

namespace SGHSS_Uninter.Api.Servicos
{
    public interface IUsuarioServico
    {
        Task<ResultadoOperacao<bool>> CriarUsuarioAsync(UsuarioNovoDTO usuarioDTO);
        Task<ResultadoOperacao<Sessao>> LoginAsync(string usuario, string senha);
        public Task<ResultadoOperacao<bool>> LogoutAsync(string token);
        Task<ResultadoOperacao<Sessao>> ObterInformacoesSessaoAsync(string token);
    }

    public class UsuarioServico : BaseServico, IUsuarioServico
    {
        private readonly UsuarioDAO _usuarioDAO;

        public UsuarioServico(
            IConfiguration configuration, 
            ILogger<UsuarioServico> logger,
            UsuarioDAO usuarioDAO) : base(configuration, logger)
        {
            _usuarioDAO = usuarioDAO;
        }

        public async Task<ResultadoOperacao<bool>> CriarUsuarioAsync(UsuarioNovoDTO usuarioDTO)
        {
            try
            {
                var usuario = new Usuario(usuarioDTO);

                var resultado = await _usuarioDAO.VerificarInserirUsuario(usuario);

                if (!resultado.Sucesso)
                {
                    return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha(resultado.Mensagem));
                }

                _logger.LogInformation($"Iniciando criação de usuário: {usuarioDTO.Email} e email: {usuarioDTO.Email}");

                await _usuarioDAO.Adicionar(usuario);

                return await Task.FromResult(ResultadoOperacao<bool>.CriarSucesso());

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao criar usuário");
                return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha("Erro ao criar usuário"));
            }
        }

        public async Task<ResultadoOperacao<Sessao>> LoginAsync(string usuario, string senha)
        {
            try
            {
                usuario = usuario.ToUpper();
                var senhaCripto = UtilitarioDeCriptografia.CriptografarString(senha);
                var chaveUsuario = await _usuarioDAO.VerificarUsuario(usuario, senhaCripto);

                if (!string.IsNullOrEmpty(chaveUsuario))
                {
                    var objetoUsuario = await _usuarioDAO.ObterPorChave(chaveUsuario);

                    using (var sessaoDAO = new SessaoDAO(_configuration))
                    {
                        var sessao = new Sessao()
                        {
                            ChaveUsuario = chaveUsuario,
                            Usuario = objetoUsuario,
                        };

                        await sessaoDAO.IncluirNovaSessao(sessao);

                        _logger.LogInformation($"Dados corretos de login para usuário :{usuario}");
                        return await Task.FromResult(ResultadoOperacao<Sessao>.CriarSucesso(sessao));
                    }
                }

                _logger.LogInformation($"Dados incorretos para tentativa de login de usuário :{usuario}");
                return await Task.FromResult(ResultadoOperacao<Sessao>.CriarFalha("Dados incorretos"));

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço de autenticação");
                return await Task.FromResult(ResultadoOperacao<Sessao>.CriarFalha("Erro no serviço de autenticação"));
            }
        }

        public async Task<ResultadoOperacao<bool>> LogoutAsync(string token)
        {
            try
            {
                using (var sessaoDAO = new SessaoDAO(_configuration))
                {
                    var sessao = await sessaoDAO.ObterSessao(token);

                    if (sessao == null)
                    {
                        return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha("Sessão não encontrada ou inativa"));
                    }

                    await sessaoDAO.SairSessao(sessao);
                    return await Task.FromResult(ResultadoOperacao<bool>.CriarSucesso());

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço de autenticação");
                return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha("Erro no serviço de autenticação"));
            }
        }

        public async Task<ResultadoOperacao<Sessao>> ObterInformacoesSessaoAsync(string token)
        {
            try
            {
                using(var sessaoDAO = new SessaoDAO(_configuration))
                {
                    var sessao = await sessaoDAO.ObterSessao(token);

                    if (sessao != null)
                    {
                        return await Task.FromResult(ResultadoOperacao<Sessao>.CriarSucesso(sessao));
                    }

                    return await Task.FromResult(ResultadoOperacao<Sessao>.CriarFalha("token inválido"));
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar sessão");
                return await Task.FromResult(ResultadoOperacao<Sessao>.CriarFalha("Erro ao verificar sessão"));
            }
        }
    }
}
