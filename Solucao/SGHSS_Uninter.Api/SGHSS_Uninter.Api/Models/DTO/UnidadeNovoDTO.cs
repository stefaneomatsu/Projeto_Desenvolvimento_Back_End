using System.ComponentModel.DataAnnotations;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class UnidadeNovoDTO
    {
        [Required]
        public int Codigo { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [MinLength(14)]
        [MaxLength(14)]
        public string Cnpj { get; set; }

        [Required]
        public string Endereco { get; set; }
    }
}
