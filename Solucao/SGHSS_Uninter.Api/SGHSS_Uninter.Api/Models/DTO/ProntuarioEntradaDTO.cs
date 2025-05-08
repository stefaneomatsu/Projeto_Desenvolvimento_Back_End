using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class ProntuarioEntradaDTO : BaseDTO
    {

        public ProntuarioEntradaDTO() { }

        public ProntuarioEntradaDTO(ProntuarioEntrada prontuarioEntrada)
        {
            this.NomeProfissional = prontuarioEntrada.ProfissionalAssociado.Nome;
            this.Tipo = prontuarioEntrada.TipoEntrada.Descricao;
            this.DataEntrada = prontuarioEntrada.DataEntrada;
            this.Titulo = prontuarioEntrada.Titulo;
            this.Descricao = prontuarioEntrada.Descricao;
            this.Anexo = prontuarioEntrada.Anexo;
        }

        public string NomeProfissional { get; set; }

        public string Tipo { get; set; }

        public DateTime DataEntrada { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public object? Anexo { get; set; }
    }
}
