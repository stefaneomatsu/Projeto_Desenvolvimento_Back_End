using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class ProntuarioDTO : BaseDTO
    {
        public ProntuarioDTO() { }

        public ProntuarioDTO(Prontuario prontuario)
        {
            this.Chave = prontuario.Chave;
            this.NomePaciente = prontuario.PacienteAssociado.Nome;
            this.HistoricoMedico = prontuario.HistoricoMedico;
            this.Alergias = prontuario.Alergias;
            this.Medicamentos = prontuario.Medicamentos;
            this.Cirurgias = prontuario.Cirurgias;

            if (prontuario.ProntuarioEntradasAssociadas != null &&
                prontuario.ProntuarioEntradasAssociadas.Count > 0)
            {
                prontuarioEntradas = new List<ProntuarioEntradaDTO>();
                prontuario.ProntuarioEntradasAssociadas.ForEach(x =>
                {
                    prontuarioEntradas.Add(new ProntuarioEntradaDTO(x));
                });
            }
        }

        public string Chave { get; set; }

        public string NomePaciente { get; set; }

        public string HistoricoMedico { get; set; }

        public string Alergias { get; set; }

        public string Medicamentos { get; set; }

        public string Cirurgias { get; set; }


        public List<ProntuarioEntradaDTO> prontuarioEntradas { get; set; }
    }
}
