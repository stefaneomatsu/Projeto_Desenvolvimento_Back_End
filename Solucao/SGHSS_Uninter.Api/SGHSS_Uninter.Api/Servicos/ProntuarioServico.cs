using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.Servicos
{
    public interface IProntuarioServico
    {
        Task<ResultadoOperacao<bool>> CriarProntuarioAsync(ProntuarioNovoDTO prontuarioNovo);
        Task<ResultadoOperacao<bool>> VincularProntuarioEntradaAsync(ProntuarioEntradaNovoDTO prontuarioEntradaNovo);
        Task<ResultadoOperacao<Prontuario>> ObterProntuarioPacienteAsync(string cpfPaciente);
    }

    public class ProntuarioServico : BaseServico, IProntuarioServico
    {
        private readonly ProntuarioDAO _prontuarioDAO;
        private readonly ProntuarioEntradaDAO _prontuarioEntradaDAO;

        public ProntuarioServico(
            IConfiguration configuration,
            ILogger<UsuarioServico> logger,
            ProntuarioDAO prontuarioDAO,
            ProntuarioEntradaDAO prontuarioEntradaDAO) : base(configuration, logger)
        {
            _prontuarioDAO = prontuarioDAO;
            _prontuarioEntradaDAO = prontuarioEntradaDAO;
        }

        public async Task<ResultadoOperacao<bool>> CriarProntuarioAsync(ProntuarioNovoDTO prontuarioNovo)
        {
            try
            {
                using (var pacienteDAO = new PacienteDAO(_configuration))
                {
                    var paciente = await pacienteDAO.ObterPacienteCpf(prontuarioNovo.CPFPaciente);

                    if (!paciente.Sucesso)
                    {
                        return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha(paciente.Mensagem));
                    }

                    var prontuario = new Prontuario(prontuarioNovo, 
                                paciente.Dados);

                    await _prontuarioDAO.Adicionar(prontuario);

                    return await Task.FromResult(ResultadoOperacao<bool>.CriarSucesso());

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao criar Prontuário");
                return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha("Erro ao criar Prontuário"));
            }
        }

        public async Task<ResultadoOperacao<Prontuario>> ObterProntuarioPacienteAsync(string cpfPaciente)
        {
            try
            {
                using (var pacienteDAO = new PacienteDAO(_configuration))
                {
                    var paciente = await pacienteDAO.ObterPacienteCpf(cpfPaciente);

                    if (!paciente.Sucesso)
                    {
                        return await Task.FromResult(ResultadoOperacao<Prontuario>.CriarFalha(paciente.Mensagem));
                    }

                    var prontuario = await _prontuarioDAO.ObterProntuarioPorPaciente(paciente.Dados);

                    return prontuario;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao obter Prontuário");
                return await Task.FromResult(ResultadoOperacao<Prontuario>.CriarFalha("Erro ao obter Prontuário"));
            }

        }

        public async Task<ResultadoOperacao<bool>> VincularProntuarioEntradaAsync(ProntuarioEntradaNovoDTO prontuarioEntradaNovo)
        {
            try
            {
                var prontuario = await _prontuarioDAO.ObterPorChave(prontuarioEntradaNovo.ChaveProntuario);

                if (prontuario == null)
                {
                    return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha("Prontuário não encontrado"));
                }

                using (var profissionalDAO = new ProfissionalDAO(_configuration))
                {
                    var profissional = await profissionalDAO.ObterProfissionalPorCPF(prontuarioEntradaNovo.CPFProfissional);

                    if (!profissional.Sucesso)
                    {
                        return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha(profissional.Mensagem));
                    }

                    var prontuarioEntrada = new ProntuarioEntrada(prontuarioEntradaNovo,
                                                    prontuario,
                                                    profissional.Dados);

                    await _prontuarioEntradaDAO.Adicionar(prontuarioEntrada);

                    return await Task.FromResult(ResultadoOperacao<bool>.CriarSucesso());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao vincular Prontuário");
                return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha("Erro ao vincular Prontuário"));
            }

        }
    }
}
