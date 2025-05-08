namespace SGHSS_Uninter.Api.Enumeradores
{
    public class TipoExame : EnumeradorSeguro<TipoExame, int>
    {
        public static readonly TipoExame RaioX =
           new((int)EnumTipoExame.RAIO_X, "Raio-X");

        public static readonly TipoExame Hemograma =
           new((int)EnumTipoExame.HEMOGRAMA, "Hemograma");

        private TipoExame(int valor, string descricao) : base(valor, descricao) { }

        public static TipoExame ObterPorValor(int valor)
        {
            return ObterPorValorBase(valor);
        }
    }
}
