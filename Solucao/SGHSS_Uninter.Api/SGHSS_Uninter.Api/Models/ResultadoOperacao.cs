namespace SGHSS_Uninter.Api.Models
{
    public class ResultadoOperacao<T>
    {
        public bool Sucesso { get; private set; }
        public string Mensagem { get; private set; }
        public T Dados { get; private set; }

        public ResultadoOperacao(bool sucesso, string mensagem, T dados)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
            Dados = dados;
        }

        public static ResultadoOperacao<T> CriarSucesso(T dados)
        {
            return new ResultadoOperacao<T>(true, null, dados);
        }

        public static ResultadoOperacao<T> CriarFalha(string mensagem)
        {
            return new ResultadoOperacao<T>(false, mensagem, default);
        }

        public static ResultadoOperacao<bool> CriarSucesso()
        {
            return new ResultadoOperacao<bool>(true, null, true);
        }

        public static ResultadoOperacao<bool> CriarFalha()
        {
            return new ResultadoOperacao<bool>(false, null, default);
        }
    }
}
