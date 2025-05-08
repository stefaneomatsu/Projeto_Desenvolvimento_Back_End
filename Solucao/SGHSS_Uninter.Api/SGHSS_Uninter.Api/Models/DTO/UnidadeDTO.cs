using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.Models.DTO
{
    public class UnidadeDTO : BaseDTO
    {
        public UnidadeDTO() { }

        public UnidadeDTO(UnidadeHospitalar unidade)
        {
            this.Codigo = unidade.Codigo;
            this.Nome = unidade.Nome;
        }

        public int Codigo { get; set; }

        public string Nome { get; set; }
    }
}
