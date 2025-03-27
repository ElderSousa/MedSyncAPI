using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using MedSync.Infrastructure.Repositories.Scripts;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace MedSync.Infrastructure.Repositories;

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

    public async Task<Pessoa?> GetIdAsync(Guid id)
    {
        var sql = $"{PessoaScripts.SelectBase}{PessoaScripts.WhereId}";
        var parametro = new { Id = id };
        try
        {
            return await GenericGetOne<Pessoa>(sql, parametro);
        }
        catch (DbException)
        {
            throw;
        }
    }

    public async Task<Pessoa?> GetCPFAsync(string Cpf)
    {
        var sql = $"{PessoaScripts.SelectBase}{PessoaScripts.WhereCPF}";
        var parametro = new { CPF = Cpf };
        try
        {
            return await GenericGetOne<Pessoa>(sql, parametro);
        }
        catch (DbException)
        {
            throw;
        }
    }

    public async Task<bool> UpdateAsync(Pessoa pessoa)
    {
        var sql = PessoaScripts.Update;
        try
        {
            var resp =  await GenericExecuteAsync(sql, pessoa);
            return resp;
        }
        catch (DbException)
        {
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var sql = PessoaScripts.Delete;
        var parametros = new { Id = id, ModificadoEm = DataHoraAtual()};
        try
        {
            return await GenericExecuteAsync(sql, parametros);
        }
        catch (DbException)
        {
            throw;
        }
    }

    public bool Existe(Guid id)
    {
        var sql = PessoaScripts.Existe;
        var parametros = new { Id = id };
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
        var sql = PessoaScripts.CPFExiste;
        var parametros = new { CPF };
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
