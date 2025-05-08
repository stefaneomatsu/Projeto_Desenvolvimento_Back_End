using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.Servicos
{
    public interface IConsultaServico
    {
        Task<ResultadoOperacao<bool>> CriarConsultaAsync(ConsultaNovoDTO consultaNovo);
        Task<ResultadoOperacao<List<Consulta>>> ObterConsultasPorCPFPacienteAsync(string cpf);
        Task<ResultadoOperacao<List<Consulta>>> ObterConsultasPorCPFProfissionalAsync(string cpf);
        Task<ResultadoOperacao<List<Consulta>>> ObterConsultasPorTokenAsync(string token);
    }

    public class ConsultaServico : BaseServico, IConsultaServico
    {

        private readonly ConsultaDAO _consultaDAO;

        public ConsultaServico(
            IConfiguration configuration,
            ILogger<UsuarioServico> logger,
            ConsultaDAO consultaDAO) : base(configuration, logger)
        {
            _consultaDAO = consultaDAO;
        }

        public async Task<ResultadoOperacao<bool>> CriarConsultaAsync(ConsultaNovoDTO consultaNovo)
        {
            try
            {
                using(var unidadeDAO = new UnidadeDAO(_configuration))
                {
                    var unidade = await unidadeDAO.ObtenhaUnidade(consultaNovo.CodigoUnidade);

                    if (!unidade.Sucesso)
                    {
                        return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha(unidade.Mensagem));
                    }

                    using (var profissionalDAO = new ProfissionalDAO(_configuration))
                    {
                        var profissional = await profissionalDAO.ObterProfissionalPorCPF(consultaNovo.CPFProfissional);

                        if (!profissional.Sucesso)
                        {
                            return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha(profissional.Mensagem));
                        }

                        using (var pacienteDAO = new PacienteDAO(_configuration))
                        {
                            var paciente = await pacienteDAO.ObterPacienteCpf(consultaNovo.CPFPaciente);

                            if (!paciente.Sucesso)
                            {
                                return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha(paciente.Mensagem));
                            }

                            var consulta = new Consulta(consultaNovo,
                                unidade.Dados,
                                profissional.Dados,
                                paciente.Dados);

                            await _consultaDAO.Adicionar(consulta);

                            return await Task.FromResult(ResultadoOperacao<bool>.CriarSucesso());

                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao criar Consulta");
                return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha("Erro ao criar Consulta"));
            }
        }

        public async Task<ResultadoOperacao<List<Consulta>>> ObterConsultasPorCPFPacienteAsync(string cpf)
        {
            try
            {
                using (var pacienteDAO = new PacienteDAO(_configuration))
                {
                    var paciente = await pacienteDAO.ObterPacienteCpf(cpf);

                    if (!paciente.Sucesso)
                    {
                        return await Task.FromResult(ResultadoOperacao<List<Consulta>>.CriarFalha(paciente.Mensagem));
                    }

                    var consultas = await _consultaDAO.ObterConsultasPorPaciente(paciente.Dados);

                    return consultas;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao obter Consulta");
                return await Task.FromResult(ResultadoOperacao<List<Consulta>>.CriarFalha("Erro ao obter Consulta"));
            }
        }

        public async Task<ResultadoOperacao<List<Consulta>>> ObterConsultasPorCPFProfissionalAsync(string cpf)
        {
            try
            {
                using (var profissionalDAO = new ProfissionalDAO(_configuration))
                {
                    var profissional = await profissionalDAO.ObterProfissionalPorCPF(cpf);

                    if (!profissional.Sucesso)
                    {
                        return await Task.FromResult(ResultadoOperacao<List<Consulta>>.CriarFalha(profissional.Mensagem));
                    }

                    var consultas = await _consultaDAO.ObterConsultasPorProfissional(profissional.Dados);

                    return consultas;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao obter Consulta");
                return await Task.FromResult(ResultadoOperacao<List<Consulta>>.CriarFalha("Erro ao obter Consulta"));
            }
        }

        public async Task<ResultadoOperacao<List<Consulta>>> ObterConsultasPorTokenAsync(string token)
        {
            try
            {
                using (var sessaoDAO = new SessaoDAO(_configuration))
                {
                    var sessao = await sessaoDAO.ObterSessao(token);

                    if (sessao == null)
                    {
                        return await Task.FromResult(ResultadoOperacao<List<Consulta>>.CriarFalha("Sessão não encontrada ou está inativa"));
                    }
                    
                    using (var usuarioDAO = new UsuarioDAO(_configuration))
                    {
                        var itemAssociado = await usuarioDAO.ObtenhaObjetoAssociadoUsuario(sessao.Usuario.Chave);
                        var itemConvertido = itemAssociado.Dados as Paciente;

                        if (itemConvertido != null)
                        {
                            var consultas = await _consultaDAO.ObterConsultasPorPaciente(itemConvertido);

                            if (!consultas.Sucesso)
                                return await Task.FromResult(ResultadoOperacao<List<Consulta>>.CriarFalha(consultas.Mensagem));

                            return consultas;

                        }

                        return await Task.FromResult(ResultadoOperacao<List<Consulta>>.CriarFalha("Usuário não é paciente"));

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao obter Consulta");
                return await Task.FromResult(ResultadoOperacao<List<Consulta>>.CriarFalha("Erro ao obter Consulta"));
            }
        }
    }
}
