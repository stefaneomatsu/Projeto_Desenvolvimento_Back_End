using SGHSS_Uninter.Api.DAO;
using SGHSS_Uninter.Api.Models.DTO;
using SGHSS_Uninter.Api.Models;
using SGHSS_Uninter.Api.Models.Persistente;

namespace SGHSS_Uninter.Api.Servicos
{
    public interface  IPlanoSaudeServico
    {
        Task<ResultadoOperacao<bool>> CriarPlanoSaudeAsync(PlanoSaudeNovoDTO planoSaudeNovo);
        Task<ResultadoOperacao<bool>> VincularPacientePlano(PacientePlanoSaudeNovoDTO pacientePlanoSaudeNovo);
    }

    public class PlanoSaudeServico : BaseServico, IPlanoSaudeServico
    {
        private readonly PlanoSaudeDAO _planoSaudeDAO;

        public PlanoSaudeServico(
            IConfiguration configuration,
            ILogger<UsuarioServico> logger,
            PlanoSaudeDAO planoSaudeDAO) : base(configuration, logger)
        {
            _planoSaudeDAO = planoSaudeDAO;
        }

        public async Task<ResultadoOperacao<bool>> CriarPlanoSaudeAsync(PlanoSaudeNovoDTO planoSaudeNovo)
        {
            try
            {
                var plano = new PlanoSaude(planoSaudeNovo);
                var resultado = await _planoSaudeDAO.VerificarInserirPlanoSaude(plano);

                if (!resultado.Sucesso)
                {
                    return resultado;
                }

                await _planoSaudeDAO.Adicionar(plano);

                return ResultadoOperacao<bool>.CriarSucesso();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao criar Plano de Saúde");
                return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha("Erro ao criar Plano de Saúde"));
            }
        }

        public async Task<ResultadoOperacao<bool>> VincularPacientePlano(PacientePlanoSaudeNovoDTO pacientePlanoSaudeNovo)
        {
            try
            {
                var plano = await _planoSaudeDAO.ObterPlanoRegistroAns(pacientePlanoSaudeNovo.RegistroANS);
                
                if (plano.Sucesso)
                {
                    using (var pacienteDAO = new PacienteDAO(_configuration))
                    {
                        var paciente = await pacienteDAO.ObterPacienteCpf(pacientePlanoSaudeNovo.CPF);

                        if (paciente.Sucesso)
                        {
                            var vinculo = new PacientesPlanoSaude(paciente.Dados.Chave, plano.Dados.Chave, pacientePlanoSaudeNovo.NumeroCarteira);

                            using (var vinculoDAO = new PacientesPlanoSaudeDAO(_configuration))
                            {
                                var resultado = await vinculoDAO.VerificarAdicionarVinculo(vinculo);

                                if (resultado.Sucesso)
                                {
                                    await vinculoDAO.Adicionar(vinculo);
                                    return ResultadoOperacao<bool>.CriarSucesso();
                                }
                                else
                                {
                                    return resultado;
                                }
                            }
                        }
                        else
                        {
                            return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha(paciente.Mensagem));
                        }

                    }
                    
                }

                return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha("Plano não encontrado"));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no serviço ao vincular paciente ao plano");
                return await Task.FromResult(ResultadoOperacao<bool>.CriarFalha("Erro ao vincular paciente ao plano"));
            }
        }
    }
}
