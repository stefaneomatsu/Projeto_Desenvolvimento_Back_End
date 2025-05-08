using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;
using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.Enumeradores;
using SGHSS_Uninter.Api.Models.Persistente;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Reflection;

namespace SGHSS_Uninter.Api.DAO
{
    public abstract class BaseDAO<T> : DBContexto<T>
        where T : new ()
    {
        public BaseDAO(IConfiguration configuration) : base(configuration)
        {
            
        }


        public virtual async Task Excluir(string chave) => AlterarStatusRegistro(chave, EnumStatusRegistro.EXCLUIDO);

        public virtual async Task Inativar(string chave) => AlterarStatusRegistro(chave, EnumStatusRegistro.INATIVO);

        public virtual async Task Ativar(string chave) => AlterarStatusRegistro(chave, EnumStatusRegistro.ATIVO);

        private async Task AlterarStatusRegistro(string chave, EnumStatusRegistro status)
        {
            SQL = $"UPDATE {NomeTabela()} SET status = @status, data_alteracao = @data_alteracao WHERE chave = @chave";

            Parametros = new Dictionary<string, object>
            {
                ["@status"] = (int)status,
                ["@chave"] = chave.ToString(),
                ["@data_alteracao"] = DateTime.UtcNow
            };

            await base.ExecutarComando();
        }

    }
}
