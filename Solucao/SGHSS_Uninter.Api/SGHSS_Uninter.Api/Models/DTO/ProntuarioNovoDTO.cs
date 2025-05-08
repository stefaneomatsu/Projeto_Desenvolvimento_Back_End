using System.ComponentModel.DataAnnotations;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class ProntuarioNovoDTO
    {
        public ProntuarioNovoDTO() { }

        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        public string CPFPaciente { get; set; }

        public string? HistoricoMedico { get; set; }

        public string? Alergias { get; set; }

        public string? Medicamentos { get; set; }

        public string? Cirurgias { get; set; }
    }
}
