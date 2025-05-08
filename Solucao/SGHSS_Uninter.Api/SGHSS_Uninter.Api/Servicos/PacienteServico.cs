using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.Servicos
{
    public interface IPacienteServico
    {
        Task<ResultadoOperacao<bool>> CriarPlanoSaudeAsync(PacienteNovoDTO pacienteNovo);
        Task<ResultadoOperacao<Paciente>> ObterPaciente(string cpf);
        Task<ResultadoOperacao<List<Paciente>>> ObterPacientes();
    }

    public class PacienteServico : BaseServico, IPacienteServico
    {
        private readonly PacienteDAO _pacienteDAO;

        public PacienteServico(
            IConfiguration configuration,
            ILogger<UsuarioServico> logger,
            PacienteDAO pacienteDAO) : base(configuration, logger)
        {
            _pacienteDAO = pacienteDAO;
        }

        public async Task<ResultadoOperacao<bool>> CriarPlanoSaudeAsync(PacienteNovoDTO pacienteNovo)
        {
            try
            {
                var paciente = new Paciente(pacienteNovo);
                var resultado = await _pacienteDAO.VerificarInserirPaciente(paciente);

                if (!resultado.Sucesso)
                {
                    return resultado;
                }

                await _pacienteDAO.AdicionarPaciente(paciente);

                return ResultadoOperacao<bool>.CriarSucesso();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao criar Paciente");
                return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha("Erro ao criar Paciente"));
            }
        }

        public async Task<ResultadoOperacao<Paciente>> ObterPaciente(string cpf)
        {
            try
            {
                return await _pacienteDAO.ObterPacienteCpf(cpf);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao buscar Paciente");
                return await Task.FromResult(ResultadoOperacao<Paciente>.CriarFalha("Erro ao buscar Paciente"));
            }
        }

        public async Task<ResultadoOperacao<List<Paciente>>> ObterPacientes()
        {
            try
            {
                var pacientes = await _pacienteDAO.ObterTodos();
                return await Task.FromResult(ResultadoOperacao<List<Paciente>>.CriarSucesso(pacientes));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao buscar Paciente");
                return await Task.FromResult(ResultadoOperacao<List<Paciente>>.CriarFalha("Erro ao buscar Pacientes"));
            }
        }
    }
}
