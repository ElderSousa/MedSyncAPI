using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using MySqlConnector;

namespace MedSync.Infrastructure.Repositories.Scripts
{
    public class PessoaRepository : BaseRepository, IPessoaRepository
    {
        public PessoaRepository(MySqlConnection mySqlConnection, IHttpContextAccessor httpContextAccessor) : base(mySqlConnection, httpContextAccessor) { }

        public async Task<bool> CreateAsync(Pessoa pessoa)
        {
            var sql = PessoaScripts.Insert;
            try
            {
                return await GenericExecuteAsync(sql, pessoa);
            }
            catch (DbException)
            {
                throw;
            }
        }

        public bool Existe(Guid id, string CPF)
        {
            var sql = PessoaScripts.Existe;
            var parametros = new {Id = id, CPF = CPF}; 
            try
            {
                return JaExiste(sql, parametros);
            }
            catch (DbException)
            {
                throw;
            }
        }
    }
}
