using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class PacienteDTO : BaseDTO
    {
        public PacienteDTO(Paciente paciente)
        {
            this.Nome = paciente.Nome;
            this.Nascimento = paciente.Nascimento;
        }

        public string Nome { get; set; }

        public DateOnly Nascimento { get; set; }
    }
}
