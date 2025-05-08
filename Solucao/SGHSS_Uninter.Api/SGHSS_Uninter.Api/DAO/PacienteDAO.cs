using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.DAO
{
    public class PacienteDAO : BaseDAO<Paciente>
    {
        public PacienteDAO(IConfiguration configuration)
       : base(configuration)
        {

        }

        public async Task<ResultadoOperacao<bool>> VerificarInserirPaciente(Paciente paciente)
        {
            // Verifica usuário
            using (var usuarioDAO = new UsuarioDAO(Configuration))
            {
                var resultadoUsuario = await usuarioDAO.VerificarInserirUsuario(paciente.UsuarioAssociado);

                if (!resultadoUsuario.Sucesso)
                {
                    return resultadoUsuario;
                }
            }

            var resultado = await VerificarExisteCPF(paciente.CPF);
            if (resultado.Sucesso)
            {
                return ResultadoOperacao<bool>.CriarFalha("Paciente já cadastrado");
            }

            return ResultadoOperacao<bool>.CriarSucesso();
        }

        public async Task<ResultadoOperacao<bool>> VerificarExisteCPF(string cpf)
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

        public async Task AdicionarPaciente(Paciente paciente)
        {
            using (var usuarioDAO = new UsuarioDAO(Configuration))
            {
                await usuarioDAO.Adicionar(paciente.UsuarioAssociado);
                await Adicionar(paciente);
            }
        }

        public async Task<ResultadoOperacao<Paciente>> ObterPacienteCpf(string cpf)
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
                return ResultadoOperacao<Paciente>.CriarSucesso(retorno[0]);
            }

            return ResultadoOperacao<Paciente>.CriarFalha("Paciente não encontrado");
        }

        public async Task<ResultadoOperacao<Paciente>> ObterPacientePorUsuario(string chaveUsuario)
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
                return ResultadoOperacao<Paciente>.CriarSucesso(retorno[0]);
            }

            return ResultadoOperacao<Paciente>.CriarFalha("Paciente não encontrado");
        }

    }
}
