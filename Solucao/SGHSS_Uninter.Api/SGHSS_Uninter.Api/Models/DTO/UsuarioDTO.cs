using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class UsuarioDTO : BaseDTO
    {

        public UsuarioDTO(Usuario usuario)
        {
            NomeUsuario = usuario.NomeUsuario;
            Email = usuario.Email;
        }

        public string NomeUsuario { get; set; }

        public string Email { get; set; }
    }
}
