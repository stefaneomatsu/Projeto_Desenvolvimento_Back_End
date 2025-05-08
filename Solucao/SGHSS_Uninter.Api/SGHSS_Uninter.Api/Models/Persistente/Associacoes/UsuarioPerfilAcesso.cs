using SGHSS_Uninter.Api.Atributos;

namespace SGHSS_Uninter.Api.Models.Persistente.Associacoes
{
    [TabelaBD("UsuarioPerfilAcesso")]
    public class UsuarioPerfilAcesso : BaseObjetoPersistente
    {

        public UsuarioPerfilAcesso() { }

        [ColunaBD("usuario")]
        public string ChaveUsuario { get; set; }

        [ColunaBD("perfil_acesso")]
        public int CodigoPerfilAcesso { get; set; }

    }
}
