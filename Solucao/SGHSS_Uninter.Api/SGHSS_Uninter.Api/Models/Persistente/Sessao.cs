using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.Utilitarios;

namespace SGHSS_Uninter.Api.Models.Persistente
{
    [TabelaBD("Sessao")]
    public class Sessao : BaseObjetoPersistente
    {
        public Sessao()
        {
            Token = UtilitarioDeStrings.GerarTokem();
        }

        public Sessao(string token)
        {
            Token = token;
        }

        [ColunaBD("token")]
        [CampoIndiceBD]
        public string Token { get; set; }

        [ColunaBD("usuario")]
        public string ChaveUsuario { get; set; }

        [ColunaBD("data_saida")]
        public DateTime? DataSaida { get; set; }

        [ColunaBDObjeto("usuario")]
        public virtual Usuario Usuario { get; set; }
    }
}
