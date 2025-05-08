using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Models.Persistente.Associacoes;

namespace SGHSS_Uninter.Api.Models.Persistente
{
    [TabelaBD("Prontuarios")]
    public class Prontuario : BaseObjetoPersistente
    {
        public Prontuario() { }

        public Prontuario(ProntuarioNovoDTO prontuarioNovo,
            Paciente paciente)
        {
            this.ChavePaciente = paciente.Chave;
            this.HistoricoMedico = prontuarioNovo.HistoricoMedico.ToUpper();
            this.Alergias = prontuarioNovo.Alergias.ToUpper();
            this.Medicamentos = prontuarioNovo.Medicamentos.ToUpper();
            this.Cirurgias = prontuarioNovo.Cirurgias.ToUpper();

            this.PacienteAssociado = paciente;
        }

        [ColunaBD("paciente")]
        public string ChavePaciente { get; set; }

        [ColunaBD("historico_medico")]
        public string HistoricoMedico { get; set; }

        [ColunaBD("alergias")]
        public string Alergias { get; set; }

        [ColunaBD("medicamentos")]
        public string Medicamentos { get; set; }

        [ColunaBD("cirurgias")]
        public string Cirurgias { get; set; }

        [ColunaBDObjeto("paciente")]
        [CampoIndiceBD]
        public virtual Paciente PacienteAssociado { get; set; }

        [AssociacaoBD(typeof(ProntuarioEntradaAssociacao), "ChaveProntuario", "ChaveProntuarioEntrada")]
        public virtual List<ProntuarioEntrada> ProntuarioEntradasAssociadas { get; set; }
    }
}
