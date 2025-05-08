using System.Security.Cryptography;
using System.Text;

namespace SGHSS_Uninter.Api.Utilitarios
{
    public static class UtilitarioDeStrings
    {
        public static string GerarTokem(int length = 255)
        {
            // Definir os caracteres permitidos
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_-+=[{]};:<>|./?";

            // Criar array de bytes para armazenar valores aleatórios
            byte[] randomBytes = new byte[length];

            // Gerar bytes aleatórios seguros
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            // Construir a string resultante
            var result = new StringBuilder(length);
            foreach (byte b in randomBytes)
            {
                result.Append(validChars[b % validChars.Length]);
            }

            return result.ToString();
        }

    }
}
