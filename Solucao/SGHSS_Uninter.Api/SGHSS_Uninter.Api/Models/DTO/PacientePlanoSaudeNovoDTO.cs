using System.ComponentModel.DataAnnotations;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class PacientePlanoSaudeNovoDTO
    {
        public PacientePlanoSaudeNovoDTO() { }

        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        public string CPF { get; set; }

        [Required]
        public string RegistroANS { get; set; }

        [Required]
        public string NumeroCarteira { get; set; }
    }
}
