using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.Persistente;


namespace SGHSS_Uninter.Api.Atributos
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AutorizacaoPorPermissao : Attribute, IAsyncAuthorizationFilter
    {
        private readonly EnumPerfilAcesso[] _perfilAcessosPermitidos;

        public AutorizacaoPorPermissao(params EnumPerfilAcesso[] perfilAcessos)
        {
            _perfilAcessosPermitidos = perfilAcessos;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Query.TryGetValue("token", out var tokenValues))
            {
                context.Result = new UnauthorizedObjectResult("Token é obrigatório como parâmetro");
                return;
            }

            var token = tokenValues.FirstOrDefault();

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedObjectResult("Token não pode ser vazio");
                return;
            }

            var sessaoDAO = context.HttpContext.RequestServices.GetRequiredService<SessaoDAO>();

            if (sessaoDAO != null)
            {
                var sessao = await sessaoDAO.ObterSessao(token);

                if (sessao != null)
                {
                    if (!VerificaPermissao(sessao))
                    {
                        context.Result = new UnauthorizedObjectResult("Você não pode acessar esse item");
                        return;
                    }

                    return;
                }
            }

            context.Result = new UnauthorizedObjectResult("Erro ao verificar sessão");
        }

        private bool VerificaPermissao(Sessao sessao)
        {
            if (sessao != null &&
                sessao.Usuario != null &&
                sessao.Usuario.PerfilAcesso != null)
            {
                var codigo = sessao.Usuario.PerfilAcesso.Valor;

                return _perfilAcessosPermitidos.Any(x => (int)x == codigo);
            }

            return false;
        }
    }
}
