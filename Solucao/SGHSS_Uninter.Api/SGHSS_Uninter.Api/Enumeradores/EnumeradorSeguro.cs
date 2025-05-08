using Newtonsoft.Json.Linq;
using System.Reflection;

namespace SGHSS_Uninter.Api.Enumeradores
{
    public abstract class EnumeradorSeguro<TEnum, TValor> 
        where TEnum : EnumeradorSeguro<TEnum, TValor>
    {
        public TValor Valor { get; }

        public string Descricao { get; }

        protected EnumeradorSeguro(TValor valor, string descricao)
        {
            Valor = valor;
            Descricao = descricao;
        }

        public static TEnum ObterPorValorBase(TValor valor)
        {
            var retorno = ObterTodos().FirstOrDefault(x => x.Valor.Equals(valor));

            if (retorno == null)
            {
                var valoresValidos = string.Join(", ", ObterTodos().Select(x => x.Valor));
                throw new ArgumentException(
                    $"Valor {valor} não é válido para {typeof(TEnum).Name}. " +
                    $"Valores válidos: {valoresValidos}");
            }

            return retorno;
        }

        public static IEnumerable<TEnum> ObterTodos()
        {
            return typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                        .Select(f => f.GetValue(null))
                        .Cast<TEnum>();
        }

        public override bool Equals(object obj)
        {
            return obj is TEnum outro && Valor.Equals(outro.Valor);
        }

        public override int GetHashCode()
        {
            return Valor.GetHashCode();
        }

        public override string ToString()
        {
            return Descricao;
        }
    }
}
