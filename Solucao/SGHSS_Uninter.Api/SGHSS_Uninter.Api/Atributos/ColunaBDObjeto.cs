namespace SGHSS_Uninter.Api.Atributos
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColunaBDObjeto : Attribute
    {
        public string Nome { get; }

        public ColunaBDObjeto(string nome)
        {
            Nome = nome;
        }
    }
}
