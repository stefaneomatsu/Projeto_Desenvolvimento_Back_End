namespace SGHSS_Uninter.Api.Enumeradores
{
    public class TipoConsulta : EnumeradorSeguro<TipoConsulta, int>
    {
        public static readonly TipoConsulta Presencial =
            new ((int)EnumTipoConsulta.PRESENCIAL, "Presencial");

        public static readonly TipoConsulta Teleconsulta =
           new ((int)EnumTipoConsulta.PRESENCIAL, "Teleconsulta");

        private TipoConsulta(int valor, string descricao) : base(valor, descricao) { }

        public static TipoConsulta ObterPorValor(int valor)
        {
            return ObterPorValorBase(valor);
        }
    }
}
