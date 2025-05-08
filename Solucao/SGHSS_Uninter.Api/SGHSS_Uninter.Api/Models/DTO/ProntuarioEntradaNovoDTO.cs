using System.ComponentModel.DataAnnotations;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class ProntuarioEntradaNovoDTO
    {
        public ProntuarioEntradaNovoDTO() { }

        [Required]
        public string ChaveProntuario { get; set; }

        [Required]
        public string CPFProfissional { get; set; }

        [Required]
        public int Tipo { get; set; }   

        public string? ChaveVinculada { get; set; }

        public DateTime DataEntrada { get; set; }
        
        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public object? Anexo { get; set; }

    }
}
