using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class LaboratorioDTO
    {
        public LaboratorioDTO() { }

        public LaboratorioDTO(Laboratorio laboratorio)
        {
            this.Nome = laboratorio.Nome;
        }

        public string Nome { get; set; }


    }
}
