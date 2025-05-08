using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.Persistente;
using System.Runtime.CompilerServices;

namespace SGHSS_Uninter.Api.DAO
{
    public class ProfissionalDAO : BaseDAO<Profissional>
    {
        public ProfissionalDAO(IConfiguration configuration)
        : base(configuration)
        {

        }

        public async Task<ResultadoOperacao<bool>> VerificarInserirProfissional(Profissional profissional)
        {
            // Verifica usuário
            using (var usuarioDAO = new UsuarioDAO(Configuration))
            {
                var resultado = await usuarioDAO.VerificarInserirUsuario(profissional.UsuarioAssociado);

                if (!resultado.Sucesso)
                {
                    return resultado;
                }

            }

            var resultadoCPF = await VerificarExisteProfissionalCPF(profissional.CPF);
            if (resultadoCPF.Sucesso)
            {
                return ResultadoOperacao<bool>.CriarFalha("CPF já cadastrado");
            }

            var resultadoNumeroOrgao = await VerificarExisteProfissionalNumeroOrgao(profissional.NumeroOrgaoProfissional);
            if (resultadoCPF.Sucesso)
            {
                return ResultadoOperacao<bool>.CriarFalha("Número do orgão já cadastrado");
            }

            return ResultadoOperacao<bool>.CriarSucesso();
        }

        public async Task<ResultadoOperacao<bool>> VerificarExisteProfissionalCPF(string cpf)
        {
            SQL = $"SELECT chave FROM {NomeTabela()} WHERE " +
                  $"cpf = @cpf";

            Parametros = new Dictionary<string, object>()
            {
                ["@cpf"] = cpf,
            };

            var retorno = await ExecutarConsulta();

            if (retorno != null &&
                retorno.Count > 0)
            {
                return ResultadoOperacao<bool>.CriarSucesso();
            }

            return ResultadoOperacao<bool>.CriarFalha();
        }

        public async Task<ResultadoOperacao<bool>> VerificarExisteProfissionalNumeroOrgao(string numero_orgao)
        {
            SQL = $"SELECT chave FROM {NomeTabela()} WHERE " +
                  $"nr_orgao = @nr_orgao";

            Parametros = new Dictionary<string, object>()
            {
                ["@nr_orgao"] = numero_orgao,
            };

            var retorno = await ExecutarConsulta();

            if (retorno != null &&
                retorno.Count > 0)
            {
                return ResultadoOperacao<bool>.CriarSucesso();
            }

            return ResultadoOperacao<bool>.CriarFalha();
        }

        public async Task AdicionarProfissional(Profissional profissional)
        {
            using (var usuarioDAO = new UsuarioDAO(Configuration))
            {
                await usuarioDAO.Adicionar(profissional.UsuarioAssociado);
                await Adicionar(profissional); 
            }
        }

        public async Task<ResultadoOperacao<Profissional>> ObterProfissionalPorCPF(string cpf)
        {
            SQL = $"SELECT {ColunasFormatada} FROM {NomeTabela()} WHERE " +
                  $"cpf = @cpf";

            Parametros = new Dictionary<string, object>()
            {
                ["@cpf"] = cpf,
            };

            var retorno = await ExecutarConsulta();

            if (retorno != null &&
                retorno.Count > 0)
            {
                return ResultadoOperacao<Profissional>.CriarSucesso(retorno[0]);
            }

            return ResultadoOperacao<Profissional>.CriarFalha("Profissional não encontrado");
        }

        public async Task<ResultadoOperacao<Profissional>> ObterProfissionalPorUsuario(string chaveUsuario)
        {
            SQL = $"SELECT {ColunasFormatada} FROM {NomeTabela()} WHERE " +
                  $"usuario = @chaveUsuario";

            Parametros = new Dictionary<string, object>()
            {
                ["@chaveUsuario"] = chaveUsuario,
            };

            var retorno = await ExecutarConsulta();

            if (retorno != null &&
                retorno.Count > 0)
            {
                return ResultadoOperacao<Profissional>.CriarSucesso(retorno[0]);
            }

            return ResultadoOperacao<Profissional>.CriarFalha("Profissional não encontrado");
        }
    }
}
