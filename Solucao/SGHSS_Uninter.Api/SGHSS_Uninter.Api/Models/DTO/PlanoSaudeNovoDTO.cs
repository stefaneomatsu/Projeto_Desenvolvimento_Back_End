using System.ComponentModel.DataAnnotations;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class PlanoSaudeNovoDTO
    {
        public PlanoSaudeNovoDTO() { }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string RegistroANS { get; set; }
    }
}
