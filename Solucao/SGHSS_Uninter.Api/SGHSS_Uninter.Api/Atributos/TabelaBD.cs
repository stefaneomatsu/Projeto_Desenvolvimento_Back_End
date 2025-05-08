namespace SGHSS_Uninter.Api.Atributos
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TabelaBD : Attribute
    {
        public TabelaBD(string  nome)
        {
            Nome = nome;
        }

        public string Nome { get; set; }

    }
}
