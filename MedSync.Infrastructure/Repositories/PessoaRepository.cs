using System.Data.Common;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using MedSync.Infrastructure.Repositories.Scripts;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;


namespace MedSync.Infrastructure.Repositories
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

        public bool Existe(Guid id)
        {
            var sql = PessoaScripts.Existe;
            var parametros = new {Id = id}; 
            try
            {
                return JaExiste(sql, parametros);
            }
            catch (DbException)
            {
                throw;
            }
        }
        
        public bool CPFExiste(string? CPF)
        {
            var sql = PessoaScripts.Existe;
            var parametros = new {CPF}; 
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
