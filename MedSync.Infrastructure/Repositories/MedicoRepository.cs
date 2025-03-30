using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using MedSync.Infrastructure.Repositories.Scripts;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

namespace MedSync.Infrastructure.Repositories;

public class MedicoRepository : BaseRepository, IMedicoRepository
{
    public MedicoRepository(MySqlConnection mySql, IHttpContextAccessor httpContextAccessor) : base(mySql, httpContextAccessor) { }


    public async Task<bool> CreateAsync(Medico medico)
    {
        var sql = MedicoScripts.Insert;

        return await GenericExecuteAsync(sql, medico);
    }

    public async Task<IEnumerable<Medico?>> GetAllAsync()
    {
        var sql = MedicoScripts.SelectBase;

        return await GenericGetList<Medico>(sql, null);
    }

    public async Task<Medico?> GetIdAsync(Guid id)
    {
        var sql = $"{MedicoScripts.SelectBase}{MedicoScripts.WhereId}";
        var parametro = new {Id = id};

        return await GenericGetOne<Medico>(sql, parametro);
    }

    public async Task<Medico?> GetCRMAsync(string crm)
    {
        var sql = $"{MedicoScripts.SelectBase}{MedicoScripts.WhereCRM}";
        var parametro = new { CRM = crm };

        return await GenericGetOne<Medico>(sql, parametro);
    }

    public async Task<bool> UpdateAsync(Medico medico)
    {
        var sql = MedicoScripts.Update;

        return await GenericExecuteAsync(sql, medico);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var sql = MedicoScripts.Delete;
        var paramtros = new {Id = id};

        return await GenericExecuteAsync(sql, id);
    }

    public bool CRMExiste(string? crm)
    {
        var sql = MedicoScripts.CRMExiste;
        var parametro = new { CRM = crm };

        return JaExiste(sql, parametro);
    }

    public bool Existe(Guid id)
    {
        var sql = MedicoScripts.Existe;
        var parametro = new { Id = id };

        return JaExiste(sql, parametro);
    }
}
