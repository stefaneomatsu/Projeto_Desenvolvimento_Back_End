using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.Models.DTO;

namespace SGHSS_Uninter.Api.Models.Persistente
{
    [TabelaBD("Laboratorio")]
    public class Laboratorio : BaseObjetoPersistente
    {
        public Laboratorio() { }

        public Laboratorio(LaboratorioNovoDTO laboratorioNovo)
        {
            this.Nome = laboratorioNovo.Nome.ToUpper();
            this.Endereco = laboratorioNovo.Endereco.ToUpper();
            this.CNPJ = laboratorioNovo.CNPJ.ToUpper();
        }

        [ColunaBD("nome")]
        public string Nome { get; set; }

        [ColunaBD("endereco")]
        public string Endereco { get; set; }

        [ColunaBD("cnpj")]
        [CampoIndiceBD]
        public string CNPJ { get; set; }
    }
}
