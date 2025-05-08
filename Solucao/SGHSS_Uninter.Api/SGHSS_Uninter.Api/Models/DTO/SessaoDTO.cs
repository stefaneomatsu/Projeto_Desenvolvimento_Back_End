using SGHSS_Uninter.Api.Models.Persistente;
using SGHSS_Uninter.Api.Utilitarios;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class SessaoDTO : BaseDTO
    {
        public SessaoDTO()
        {

        }

        public SessaoDTO(string token)
        {
            Token = token;
        }

        public SessaoDTO(Sessao sessao)
        {
            Token = sessao.Token;
            Usuario = new UsuarioDTO(sessao.Usuario);
        }

        public string Token { get; set; }

        public UsuarioDTO Usuario { get; set; }
    }
}
