using System.ComponentModel.DataAnnotations;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class UsuarioNovoDTO
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string NomeUsuario { get; set; }

        [Required]
        [MinLength(6)]
        public string Senha { get; set; }

        [Required]
        public int CodigoPerfil { get; set; }
    }
}
