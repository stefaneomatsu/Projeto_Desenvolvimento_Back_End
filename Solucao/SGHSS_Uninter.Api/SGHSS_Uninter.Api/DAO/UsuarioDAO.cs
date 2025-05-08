using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Models.Persistente;
using System.Data;

namespace SGHSS_Uninter.Api.DAO
{
    public class UsuarioDAO : BaseDAO<Usuario>
    {
        public UsuarioDAO(IConfiguration configuration) 
        : base(configuration)
        {
            
        }

        public async Task<ResultadoOperacao<bool>> VerificarInserirUsuario(Usuario usuario)
        {
            if (await VerificarExisteEmail(usuario.Email))
            {
                return ResultadoOperacao<bool>.CriarFalha("Email já cadastrado");
            }
            if (await VerificarExisteUsuario(usuario.NomeUsuario))
            {
                return ResultadoOperacao<bool>.CriarFalha("Usuário já cadastrado");
            }
            return ResultadoOperacao<bool>.CriarSucesso();
        }

        public async Task<string?> VerificarUsuario(string user, string senhaCriptografado)
        {
            SQL =   $"SELECT chave FROM {NomeTabela()} WHERE " +
                    $"(nome_usuario = @usuario OR email = @usuario) AND senha_criptografada = @senha";

            Parametros = new Dictionary<string, object>()
            {
                ["usuario"] = user,
                ["@senha"] = senhaCriptografado
            };

            var retorno = await ExecutarConsulta();

            if (retorno != null &&
                retorno.Count > 0)
            {
                return retorno[0].Chave;
            }

            return null;
        }

        public async Task<bool> VerificarExisteUsuario(string usuario)
        {
            SQL = $"SELECT chave FROM {NomeTabela()} WHERE " +
                  $"nome_usuario = @usuario";

            Parametros = new Dictionary<string, object>()
            {
                ["usuario"] = usuario,
            };

            var retorno = await ExecutarConsulta();

            if (retorno != null &&
                retorno.Count > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> VerificarExisteEmail(string email)
        {
            SQL = $"SELECT chave FROM {NomeTabela()} WHERE " +
                  $"email = @email";

            Parametros = new Dictionary<string, object>()
            {
                ["@email"] = email,
            };

            var retorno = await ExecutarConsulta();

            if (retorno != null &&
                retorno.Count > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<ResultadoOperacao<object>> ObtenhaObjetoAssociadoUsuario(string chaveUsuario)
        {
            var usuario = await ObterPorChave(chaveUsuario);

            if (usuario == null)
                return ResultadoOperacao<object>.CriarFalha("Usuário não encontrado");

            if (PerfilAcesso.EhPaciente(usuario.PerfilAcesso))
            {
                using (var pacienteDAO = new PacienteDAO(Configuration))
                {
                    var paciente = await pacienteDAO.ObterPacientePorUsuario(chaveUsuario);

                    if (!paciente.Sucesso)
                        return ResultadoOperacao<object>.CriarFalha("Paciente não encontrado");

                    return ResultadoOperacao<object>.CriarSucesso(paciente.Dados);
                }
            }

            else if (PerfilAcesso.EhProfissional(usuario.PerfilAcesso))
            {
                using (var profissionalDAO = new ProfissionalDAO(Configuration))
                {
                    var profissional = await profissionalDAO.ObterProfissionalPorUsuario(chaveUsuario);

                    if (!profissional.Sucesso)
                        return ResultadoOperacao<object>.CriarFalha("Profissional não encontrado");

                    return ResultadoOperacao<object>.CriarSucesso(profissional.Dados);
                }
            }

            return ResultadoOperacao<object>.CriarFalha("Usuário não está vinculado a uma função");
        }
    }
}
