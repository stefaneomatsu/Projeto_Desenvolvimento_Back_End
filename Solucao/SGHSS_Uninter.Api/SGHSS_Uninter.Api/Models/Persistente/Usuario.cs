using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Models.Persistente.Associacoes;
using SGHSS_Uninter.Api.Utilitarios;

namespace SGHSS_Uninter.Api.Models.Persistente
{
    [TabelaBD("Usuarios")]
    public class Usuario : BaseObjetoPersistente
    {

        public Usuario() { }

        public Usuario(UsuarioNovoDTO usuarioDTO)
        {
            this.Email = usuarioDTO.Email.ToUpper();
            this.NomeUsuario = usuarioDTO.NomeUsuario.ToUpper();
            this.SenhaCriptografada = UtilitarioDeCriptografia.CriptografarString(usuarioDTO.Senha);
            this.PerfilAcesso = PerfilAcesso.ObterPorValor(usuarioDTO.CodigoPerfil);
        }

        [ColunaBD("email")]
        [CampoIndiceBD]
        public string Email { get; set; }

        [ColunaBD("nome_usuario")]
        [CampoIndiceBD]
        public string NomeUsuario { get; set; }

        [ColunaBD("senha_criptografada")]
        public string SenhaCriptografada { get; set; }

        [ColunaBD("perfil_acesso")]
        public virtual PerfilAcesso PerfilAcesso { get; set; }
    }
}
