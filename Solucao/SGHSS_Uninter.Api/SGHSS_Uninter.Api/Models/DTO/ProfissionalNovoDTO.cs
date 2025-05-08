using System.ComponentModel.DataAnnotations;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class ProfissionalNovoDTO
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        public string Cpf { get; set; }

        [Required]
        public DateOnly Nascimento { get; set; }

        [Required]
        public string NumeroOrgao { get; set; }

        [Required]
        public UsuarioNovoDTO UsuarioNovo { get; set; }

    }
}
