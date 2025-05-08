using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.DTO;

namespace SGHSS_Uninter.Api.Models.Persistente
{
    [TabelaBD("ProntuariosEntradas")]
    public class ProntuarioEntrada : BaseObjetoPersistente
    {
        public ProntuarioEntrada() { }

        public ProntuarioEntrada(ProntuarioEntradaNovoDTO prontuarioEntradaNovo,
            Prontuario prontuario,
            Profissional profissional)
        {
            this.ChaveProntuario = prontuario.Chave;
            this.ChaveProfissional = profissional.Chave;
            this.TipoEntrada = TipoEntradaProntuario.ObterPorValor(prontuarioEntradaNovo.Tipo);
            this.ChaveVinculo = prontuarioEntradaNovo.ChaveVinculada;
            this.DataEntrada = prontuarioEntradaNovo.DataEntrada;
            this.Titulo = prontuarioEntradaNovo.Titulo.ToUpper();
            this.Descricao = prontuarioEntradaNovo.Descricao.ToUpper();
            this.Anexo = prontuarioEntradaNovo.Anexo;

            this.ProfissionalAssociado = profissional;
        }

        [ColunaBD("prontuario")]
        [CampoIndiceBD]
        public string ChaveProntuario { get; set; }

        [ColunaBD("profissional")]
        [CampoIndiceBD]
        public string ChaveProfissional { get; set; }

        [ColunaBD("tipo")]
        [CampoIndiceBD]
        public TipoEntradaProntuario TipoEntrada { get; set; }

        [ColunaBD("chave_vinculo")]
        public string? ChaveVinculo { get; set; }

        [ColunaBD("data_entrada")]
        [CampoIndiceBD]
        public DateTime DataEntrada { get; set; }

        [ColunaBD("titulo")]
        public string Titulo { get; set; }

        [ColunaBD("descricao")]
        public string Descricao { get; set; }

        [ColunaBD("anexo")]
        public object? Anexo { get; set; }

        [ColunaBDObjeto("profissional")]
        public virtual Profissional ProfissionalAssociado { get; set; }

    }
}
