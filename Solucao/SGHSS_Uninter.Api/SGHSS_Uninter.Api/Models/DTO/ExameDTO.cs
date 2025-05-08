using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class ExameDTO
    {
        public ExameDTO() { }

        public ExameDTO(Exame exame)
        {
            this.Chave = exame.Chave;
            this.NomePaciente = exame.PacienteAssociado.Nome;
            this.NomeProfissional = exame.ProfissionalAssociado.Nome;
            this.Tipo = exame.Tipo.Descricao;
            this.DataSolicitacao = exame.DataSolicitacao;
            this.DataRealizacao = exame.DataRealizacao;
            this.NomeLaboratorio = exame.LaboratorioAssociado.Nome;
            this.Anexo = exame.Anexo;
        }

        public string Chave { get; set; }

        public string NomePaciente { get; set; }

        public string NomeProfissional { get; set; }

        public string Tipo { get; set; }

        public DateTime DataSolicitacao { get; set; }

        public DateTime? DataRealizacao { get; set; }

        public string NomeLaboratorio { get; set; }

        public object? Anexo { get; set; }
    }
}
