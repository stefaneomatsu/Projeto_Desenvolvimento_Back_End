using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Models.Persistente.Associacoes;

namespace SGHSS_Uninter.Api.Models.Persistente
{
    [TabelaBD("Pacientes")]
    public class Paciente : BaseObjetoPersistente
    {
        public Paciente() { }

        public Paciente(PacienteNovoDTO pacienteNovo)
        {
            this.Nome = pacienteNovo.Nome.ToUpper();
            this.CPF = pacienteNovo.CPF.ToUpper();
            this.Nascimento = pacienteNovo.Nascimento;
            this.Genero = Genero.ObterPorValor(pacienteNovo.Genero);
            this.Endereco = pacienteNovo.Endereco.ToUpper();
            this.Telefone = pacienteNovo.Telefone.ToUpper();
            this.UsuarioAssociado = new Usuario(pacienteNovo.UsuarioNovo);

            this.Usuario = this.UsuarioAssociado.Chave;
            this.UsuarioAssociado.PerfilAcesso = PerfilAcesso.Paciente;
        }

        [ColunaBD("nome")]
        public string Nome { get; set; }

        [ColunaBD("cpf")]
        [CampoIndiceBD]
        public string CPF { get; set; }

        [ColunaBD("nascimento")]
        public DateOnly Nascimento { get; set; }

        [ColunaBD("genero")]
        public Genero Genero { get; set; }

        [ColunaBD("endereco")]
        public string Endereco { get; set; }

        [ColunaBD("telefone")]
        public string Telefone { get; set; }

        [ColunaBD("usuario")]
        public string Usuario { get; set; }

        [ColunaBDObjeto("usuario")]
        public virtual Usuario UsuarioAssociado { get; set; }

        [AssociacaoBD(typeof(PacientesPlanoSaude), "ChavePaciente", "Chave")]
        public virtual List<PacientesPlanoSaude> PlanoSaudesAssociados { get; set; }
    }
}
