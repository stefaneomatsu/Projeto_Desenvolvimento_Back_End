using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.Models.DTO;

namespace SGHSS_Uninter.Api.Models.Persistente
{
    [TabelaBD("Profissionais")]
    public class Profissional : BaseObjetoPersistente
    {
        public Profissional() { }

        public Profissional(ProfissionalNovoDTO profissionalNovo)
        {
            this.Nome = profissionalNovo.Nome.ToUpper();
            this.CPF = profissionalNovo.Cpf.ToUpper();
            this.Nascimento = profissionalNovo.Nascimento;
            this.NumeroOrgaoProfissional = profissionalNovo.NumeroOrgao.ToUpper();
            this.UsuarioAssociado = new Usuario(profissionalNovo.UsuarioNovo);

            this.Usuario = UsuarioAssociado.Chave;
        }

        [ColunaBD("nome")]
        public string Nome { get; set; }

        [ColunaBD("cpf")]
        [CampoIndiceBD]
        public string CPF { get; set; }

        [ColunaBD("nascimento")]
        public DateOnly Nascimento { get; set; }

        [ColunaBD("nr_orgao")]
        [CampoIndiceBD]
        public string NumeroOrgaoProfissional { get; set; }

        [ColunaBD("usuario")]
        public string Usuario { get; set; }


        [ColunaBDObjeto("usuario")]
        public virtual Usuario UsuarioAssociado { get; set; }
    }
}
