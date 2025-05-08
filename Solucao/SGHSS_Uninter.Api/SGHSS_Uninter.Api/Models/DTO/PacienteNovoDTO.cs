using SGHSS_Uninter.Api.Enumeradores;
using System.ComponentModel.DataAnnotations;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class PacienteNovoDTO
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string CPF { get; set; }

        [Required]
        public DateOnly Nascimento { get; set; }

        [Required]
        public int Genero { get; set; }

        [Required]
        public string Endereco { get; set; }

        [Required]
        public string Telefone { get; set; }

        [Required]
        public UsuarioNovoDTO UsuarioNovo { get; set; }
    }
}
