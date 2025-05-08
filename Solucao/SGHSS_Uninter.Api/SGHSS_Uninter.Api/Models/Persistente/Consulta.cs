using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.DTO;

namespace SGHSS_Uninter.Api.Models.Persistente
{
    [TabelaBD("Consultas")]
    public class Consulta : BaseObjetoPersistente
    {
        public Consulta() { }

        public Consulta(
            ConsultaNovoDTO consultaNovo,
            UnidadeHospitalar unidade,
            Profissional profissional,
            Paciente paciente)
        {
            this.ChaveUnidade = unidade.Chave;
            this.Data = consultaNovo.DataConsulta;
            this.ChaveProfissional = profissional.Chave;
            this.ChavePaciente = paciente.Chave;
            this.TipoConsulta = TipoConsulta.ObterPorValor(consultaNovo.Tipo);
            this.LinkConsulta = consultaNovo.Link.ToUpper();

            this.UnidadeAssociada = unidade;
            this.ProfissionalAssociado = profissional;
            this.PacienteAssociado = paciente;
        }

        [ColunaBD("unidade")]
        [CampoIndiceBD]
        public string ChaveUnidade { get; set; }

        [ColunaBD("data_consulta")]
        [CampoIndiceBD]
        public DateTime Data { get; set; }

        [ColunaBD("profissional")]
        [CampoIndiceBD]
        public string ChaveProfissional { get; set; }

        [ColunaBD("paciente")]
        public string ChavePaciente { get; set; }

        [ColunaBD("tipo")]
        public TipoConsulta TipoConsulta { get; set; }

        [ColunaBD("link_consulta")]
        public string LinkConsulta { get; set; }

        [ColunaBDObjeto("unidade")]
        public virtual UnidadeHospitalar UnidadeAssociada { get; set; }

        [ColunaBDObjeto("profissional")]
        public virtual Profissional ProfissionalAssociado { get; set; }

        [ColunaBDObjeto("paciente")]
        public virtual Paciente PacienteAssociado { get; set; }
    }
}
