using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.Servicos
{
    public interface IExameServico
    {
        Task<ResultadoOperacao<bool>> CriarExameAsync(ExameNovoDTO exameNovo);
        Task<ResultadoOperacao<List<Exame>>> ObterExamesPorCPFPacienteAsync(string cpf);
        Task<ResultadoOperacao<List<Exame>>> ObterExamesPorCPFProfissionalAsync(string cpf);
        Task<ResultadoOperacao<List<Exame>>> ObterExamesPorTokenAsync(string token);
    }

    public class ExameServico : BaseServico, IExameServico
    {
        private readonly ExameDAO _exameDAO;

        public ExameServico(
            IConfiguration configuration,
            ILogger<UsuarioServico> logger,
            ExameDAO exameDAO) : base(configuration, logger)
        {
            _exameDAO = exameDAO;
        }

        public async Task<ResultadoOperacao<bool>> CriarExameAsync(ExameNovoDTO exameNovo)
        {
            try
            {
                using (var pacienteDAO = new PacienteDAO(_configuration))
                {
                    var paciente = await pacienteDAO.ObterPacienteCpf(exameNovo.CPFPaciente);

                    if (!paciente.Sucesso)
                    {
                        return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha(paciente.Mensagem));
                    }

                    using (var profissionalDAO = new ProfissionalDAO(_configuration))
                    {
                        var profissional = await profissionalDAO.ObterProfissionalPorCPF(exameNovo.CPFProfissionalSolicitante);

                        if (!profissional.Sucesso)
                        {
                            return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha(profissional.Mensagem));
                        }

                        using (var laborarioDAO = new LaboratorioDAO(_configuration))
                        {
                            var laboratorio = await laborarioDAO.ObterLaboratorioPorCNPJ(exameNovo.CNPJLaboratorio);

                            if (!laboratorio.Sucesso)
                            {
                                return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha(laboratorio.Mensagem));
                            }

                            var exame = new Exame(exameNovo,
                                        paciente.Dados,
                                        profissional.Dados,
                                        laboratorio.Dados);

                            await _exameDAO.Adicionar(exame);

                            return await Task.FromResult(ResultadoOperacao<bool>.CriarSucesso());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao criar Exame");
                return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha("Erro ao criar Exame"));
            }
        }

        public async Task<ResultadoOperacao<List<Exame>>> ObterExamesPorCPFPacienteAsync(string cpf)
        {
            try
            {
                using (var pacienteDAO = new PacienteDAO(_configuration))
                {
                    var paciente = await pacienteDAO.ObterPacienteCpf(cpf);

                    if (!paciente.Sucesso)
                    {
                        return await Task.FromResult(ResultadoOperacao<List<Exame>>.CriarFalha(paciente.Mensagem));
                    }

                    var exames = await _exameDAO.ObterExamesPorPaciente(paciente.Dados);

                    return exames;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao obter Exames");
                return await Task.FromResult(ResultadoOperacao<List<Exame>>.CriarFalha("Erro ao obter Exames"));
            }
        }

        public async Task<ResultadoOperacao<List<Exame>>> ObterExamesPorCPFProfissionalAsync(string cpf)
        {
            try
            {
                using (var profissionalDAO = new ProfissionalDAO(_configuration))
                {
                    var profissional = await profissionalDAO.ObterProfissionalPorCPF(cpf);

                    if (!profissional.Sucesso)
                    {
                        return await Task.FromResult(ResultadoOperacao<List<Exame>>.CriarFalha(profissional.Mensagem));
                    }

                    var exames = await _exameDAO.ObterExamesPorProfissional(profissional.Dados);

                    return exames;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao obter Exames");
                return await Task.FromResult(ResultadoOperacao<List<Exame>>.CriarFalha("Erro ao obter Exames"));
            }
        }

        public async Task<ResultadoOperacao<List<Exame>>> ObterExamesPorTokenAsync(string token)
        {
            try
            {
                using (var sessaoDAO = new SessaoDAO(_configuration))
                {
                    var sessao = await sessaoDAO.ObterSessao(token);

                    if (sessao == null)
                    {
                        return await Task.FromResult(ResultadoOperacao<List<Exame>>.CriarFalha("Sessão não encontrada ou está inativa"));
                    }

                    using (var usuarioDAO = new UsuarioDAO(_configuration))
                    {
                        var itemAssociado = await usuarioDAO.ObtenhaObjetoAssociadoUsuario(sessao.Usuario.Chave);
                        var itemConvertido = itemAssociado.Dados as Paciente;

                        if (itemConvertido != null)
                        {
                            var consultas = await _exameDAO.ObterExamesPorPaciente(itemConvertido);

                            if (!consultas.Sucesso)
                                return await Task.FromResult(ResultadoOperacao<List<Exame>>.CriarFalha(consultas.Mensagem));

                            return consultas;

                        }

                        return await Task.FromResult(ResultadoOperacao<List<Exame>>.CriarFalha("Usuário não é paciente"));

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao obter Exames");
                return await Task.FromResult(ResultadoOperacao<List<Exame>>.CriarFalha("Erro ao obter Exames"));
            }
        }
    }
}
