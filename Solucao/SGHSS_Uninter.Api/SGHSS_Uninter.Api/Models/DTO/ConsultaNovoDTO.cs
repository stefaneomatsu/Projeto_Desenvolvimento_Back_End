using System.ComponentModel.DataAnnotations;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class ConsultaNovoDTO
    {
        [Required]
        public int CodigoUnidade { get; set; }

        [Required]
        public DateTime DataConsulta { get; set; }

        [Required]
        public int Tipo { get; set; }

        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        public string CPFProfissional { get; set; }

        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        public string CPFPaciente { get; set; }

        [Url]
        public string Link { get; set; }
    }
}
