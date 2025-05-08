using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class ConsultaDTO : BaseDTO
    {
        public ConsultaDTO() { }

        public ConsultaDTO(Consulta consulta)
        {
            this.CodigoUnidade = consulta.UnidadeAssociada.Codigo;
            this.DataConsulta = consulta.Data;
            this.Tipo = consulta.TipoConsulta.Descricao;
            this.NomeProfissional = consulta.ProfissionalAssociado.Nome;
            this.NomePaciente = consulta.PacienteAssociado.Nome;
        }

        public int CodigoUnidade { get; set; }

        public DateTime DataConsulta { get; set; }

        public string Tipo { get; set; }

        public string NomeProfissional { get; set; }

        public string NomePaciente { get; set; }
    }
}
