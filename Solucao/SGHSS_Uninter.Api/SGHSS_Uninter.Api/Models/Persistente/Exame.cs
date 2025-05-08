using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.DTO;

namespace SGHSS_Uninter.Api.Models.Persistente
{
    [TabelaBD("Exames")]
    public class Exame : BaseObjetoPersistente
    {
        public Exame() { }

        public Exame(ExameNovoDTO exameNovo,
            Paciente paciente,
            Profissional profissional,
            Laboratorio laboratorio)
        {
            this.ChavePaciente = paciente.Chave;
            this.ChaveProfissional = profissional.Chave;
            this.Tipo = TipoExame.ObterPorValor(exameNovo.TipoExame);
            this.Descricao = exameNovo.Descricao.ToUpper();
            this.DataSolicitacao = exameNovo.DataSolicitacao;
            this.DataRealizacao = exameNovo.DataRealizacao;
            this.ChaveLaboratorio = laboratorio.Chave;
            this.Anexo = exameNovo.Anexo;

            this.PacienteAssociado = paciente;
            this.ProfissionalAssociado = profissional;
            this.LaboratorioAssociado = laboratorio;
        }

        [ColunaBD("paciente")]
        [CampoIndiceBD]
        public string ChavePaciente { get; set; }

        [ColunaBD("profissional")]
        [CampoIndiceBD]
        public string ChaveProfissional { get; set; }

        [ColunaBD("tipo")]
        [CampoIndiceBD]
        public TipoExame Tipo { get; set; }

        [ColunaBD("descricao")]
        public string Descricao { get; set; }

        [ColunaBD("data_solicitacao")]
        [CampoIndiceBD]
        public DateTime DataSolicitacao { get; set; }

        [ColunaBD("data_realizacao")]
        public DateTime? DataRealizacao { get; set; }

        [ColunaBD("laboratorio")]
        public string ChaveLaboratorio { get; set; }

        [ColunaBD("anexo")]
        public object? Anexo { get; set; }

        [ColunaBDObjeto("paciente")]
        public virtual Paciente PacienteAssociado { get; set; }

        [ColunaBDObjeto("profissional")]
        public virtual Profissional ProfissionalAssociado { get; set; }

        [ColunaBDObjeto("laboratorio")]
        public virtual Laboratorio LaboratorioAssociado { get; set; }
    }
}
