using System.ComponentModel.DataAnnotations;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class ExameNovoDTO
    {
        public ExameNovoDTO() { }

        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        public string CPFPaciente { get; set; }

        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        public string CPFProfissionalSolicitante { get; set; }

        [Required]
        public int TipoExame { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public DateTime DataSolicitacao { get; set; }

        public DateTime? DataRealizacao { get; set; }

        [Required]
        [MinLength(14)]
        [MaxLength(14)]
        public string CNPJLaboratorio { get; set; }

        public object? Anexo { get; set; }
    }
}
