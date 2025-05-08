using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.Models.DTO;

namespace SGHSS_Uninter.Api.Models.Persistente
{
    [TabelaBD("PlanoSaude")]
    public class PlanoSaude : BaseObjetoPersistente
    {
        public PlanoSaude() { }

        public PlanoSaude(PlanoSaudeNovoDTO planoSaudeNovo)
        {
            this.Nome = planoSaudeNovo.Nome.ToUpper();
            this.RegistroANS = planoSaudeNovo.RegistroANS.ToUpper();
        }

        [ColunaBD("nome")]
        [CampoIndiceBD]
        public string Nome { get; set; }

        [ColunaBD("registro_ans")]
        [CampoIndiceBD]
        public string RegistroANS { get; set; }
    }
}
