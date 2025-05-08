namespace SGHSS_Uninter.Api.Enumeradores
{
    public class TipoEntradaProntuario : EnumeradorSeguro<TipoEntradaProntuario, int>
    {
        public static readonly TipoEntradaProntuario Exame =
         new((int)EnumTipoEntradaProntuario.EXAME, "Exame");

        public static readonly TipoEntradaProntuario Consulta =
           new((int)EnumTipoEntradaProntuario.CONSULTA, "Consulta");

        public static readonly TipoEntradaProntuario Observação =
           new((int)EnumTipoEntradaProntuario.OBSERVACAO, "Observação");

        private TipoEntradaProntuario(int valor, string descricao) : base(valor, descricao) { }

        public static TipoEntradaProntuario ObterPorValor(int valor)
        {
            return ObterPorValorBase(valor);
        }
    }
}
