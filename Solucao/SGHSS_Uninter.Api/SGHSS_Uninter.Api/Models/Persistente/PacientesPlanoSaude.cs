using SGHSS_Uninter.Api.Atributos;

namespace SGHSS_Uninter.Api.Models.Persistente
{
    [TabelaBD("PacientesPlanoSaude")]
    public class PacientesPlanoSaude : BaseObjetoPersistente
    {
        public PacientesPlanoSaude() { }
        
        public PacientesPlanoSaude(string chavePaciente, string chavePlanoSaude, string numeroCarteira)
        {
            this.ChavePaciente = chavePaciente;
            this.ChavePlanoSaude = chavePlanoSaude;
            this.NumeroCarteira = numeroCarteira.ToUpper();
        }

        [ColunaBD("paciente")]
        [CampoIndiceBD]
        public string ChavePaciente { get; set; }

        [ColunaBD("plano_saude")]
        [CampoIndiceBD]
        public string ChavePlanoSaude { get; set; }

        [ColunaBD("numero_carteira")]
        [CampoIndiceBD]
        public string NumeroCarteira { get; set; }
    }
}
