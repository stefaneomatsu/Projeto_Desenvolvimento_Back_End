namespace SGHSS_Uninter.Api.Enumeradores
{
    public class Genero : EnumeradorSeguro<Genero, int>
    {
        public static readonly Genero Feminino =
            new((int)EnumGenero.FEMININO, "Feminino");

        public static readonly Genero Masculino =
            new((int)EnumGenero.MASCULINO, "Masculino");

        public static readonly Genero Outro =
            new((int)EnumGenero.OUTRO, "Outro");

        private Genero(int valor, string descricao) : base(valor, descricao) { }

        public static Genero ObterPorValor(int valor)
        {
            return ObterPorValorBase(valor);
        }
    }
}
