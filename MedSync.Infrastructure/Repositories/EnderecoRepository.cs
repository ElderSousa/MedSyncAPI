using System.Data.Common;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using MedSync.Infrastructure.Repositories.Scripts;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

namespace MedSync.Infrastructure.Repositories;

public class EnderecoRepository : BaseRepository, IEnderecoRepository
{
    public EnderecoRepository(MySqlConnection mySql, IHttpContextAccessor httpContextAccessor) : base(mySql, httpContextAccessor) { }

    public async Task<bool> CreateAsync(Endereco endereco)
    {
        var sql = EnderecoScritps.Insert;

        return await GenericExecuteAsync(sql, endereco);
    }

    public async Task<Endereco?> GetIdAsync(Guid id)
    {
        var sql = $"{EnderecoScritps.SelectBase}{EnderecoScritps.WhereId}";
        var parametro = new { Id = id };

        return await GenericGetOne<Endereco>(sql, parametro);
    }

    public async Task<Endereco?> GetCEPAsync(string cep)
    {
        var sql = $"{EnderecoScritps.SelectBase}{EnderecoScritps.WhereCEP}";
        var parametro = new { CEP = cep };

        return await GenericGetOne<Endereco>(sql, parametro);
    }

    public async Task<bool> UpdateAsync(Endereco endereco)
    {
        var sql = EnderecoScritps.Update;

        return await GenericExecuteAsync(sql, endereco);
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        var sql = EnderecoScritps.Delete;
        var parametro = new { Id = id, ModificadoEm = DataHoraAtual() };

        return await GenericExecuteAsync(sql, parametro);
    }

    public bool Existe(Guid id)
    {
        var sql = EnderecoScritps.Existe;
        var parametros = new { Id = id };

        return JaExiste(sql, parametros);
    }
}
