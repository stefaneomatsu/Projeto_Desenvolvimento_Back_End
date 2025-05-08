namespace SGHSS_Uninter.Api.Atributos
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AssociacaoBD : Attribute
    {
        
        public AssociacaoBD(Type objetoAssociacao, string propriedadeEsquerda, string propriedadeDireita)
        {
            ObjetoAssociacao = objetoAssociacao;
            PropriedadeEsquerda = propriedadeEsquerda;
            PropriedadeDireita = propriedadeDireita;
        }

        public Type ObjetoAssociacao { get; set; }

        public string PropriedadeEsquerda { get; set; }

        public string PropriedadeDireita { get; set; }
    }
}
