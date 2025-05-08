using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.Models.DTO;

namespace SGHSS_Uninter.Api.Models.Persistente
{
    [TabelaBD("Unidade")]
    public class UnidadeHospitalar : BaseObjetoPersistente
    {
        public UnidadeHospitalar() { }

        public UnidadeHospitalar(UnidadeNovoDTO unidadeNovo)
        {
            this.Codigo = unidadeNovo.Codigo;
            this.Nome = unidadeNovo.Nome.ToUpper();
            this.CNPJ = unidadeNovo.Cnpj.ToUpper();
            this.Endereco = unidadeNovo.Endereco.ToUpper();
        }

        [ColunaBD("codigo")]
        [CampoIndiceBD]
        public int Codigo { get; set; }

        [ColunaBD("nome")]
        public string Nome { get; set; }

        [ColunaBD("cnpj")]
        [CampoIndiceBD]
        public string CNPJ { get; set; }

        [ColunaBD("endereco")]
        public string Endereco { get; set; }
    }
}
