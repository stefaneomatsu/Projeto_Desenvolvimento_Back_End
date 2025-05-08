namespace SGHSS_Uninter.Api.Atributos
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColunaBD : Attribute
    {
        public ColunaBD(string nome)
        {
            Nome = nome;
        }

        public string Nome { get; set; }
    }
}
