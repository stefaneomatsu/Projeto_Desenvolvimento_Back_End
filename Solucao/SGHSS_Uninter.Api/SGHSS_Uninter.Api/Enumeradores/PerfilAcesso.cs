namespace SGHSS_Uninter.Api.Enumeradores
{
    public class PerfilAcesso : EnumeradorSeguro<PerfilAcesso, int>
    {
        public static readonly PerfilAcesso Administrador = 
            new((int)EnumPerfilAcesso.ADMINISTRADOR, "Administrador");

        public static readonly PerfilAcesso Paciente =
            new((int)EnumPerfilAcesso.PACIENTE, "Paciente");

        public static readonly PerfilAcesso Medico =
           new((int)EnumPerfilAcesso.MEDICO, "Médico");

        public static readonly PerfilAcesso Enfermeiro =
          new((int)EnumPerfilAcesso.ENFERMEIRO, "Enfermeiro");

        public static readonly PerfilAcesso TecnicoEnfermagem =
            new((int)EnumPerfilAcesso.TEC_ENFERMAGEM, "Técnico de enfermagem");

        public static readonly PerfilAcesso Recepcao =
            new((int)EnumPerfilAcesso.RECEPCAO, "Recepção");

        private PerfilAcesso(int valor, string descricao) : base(valor, descricao) { }

        public static PerfilAcesso ObterPorValor(int valor)
        {
            return ObterPorValorBase(valor);
        }

        public static bool EhPaciente(PerfilAcesso perfilAcesso)
        {
            return
                perfilAcesso.Equals(PerfilAcesso.Paciente);
        }

        public static bool EhProfissional(PerfilAcesso perfilAcesso)
        {
            return
                perfilAcesso.Equals(PerfilAcesso.Medico) ||
                perfilAcesso.Equals(PerfilAcesso.Enfermeiro) ||
                perfilAcesso.Equals(PerfilAcesso.TecnicoEnfermagem);
        }
    }
}
