using Dapper;
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

        return await GetList(sql, null);
    }

    public async Task<Medico?> GetIdAsync(Guid id)
    {
        var sql = $"{MedicoScripts.SelectBase}{MedicoScripts.WhereId}";
        var parametro = new {Id = id};

        return (await GetList(sql, parametro)).FirstOrDefault();
    }

    public async Task<Medico?> GetCRMAsync(string crm)
    {
        var sql = $"{MedicoScripts.SelectBase}{MedicoScripts.WhereCRM}";
        var parametro = new { CRM = crm };

        return (await GetList(sql, parametro)).FirstOrDefault();
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

    #region MÉTODOSPRIVADOS
    private async Task<IEnumerable<Medico>> GetList(string sql, object? parametros)
    {
        var medicoDictionary = new Dictionary<Guid, Medico>();
        try
        {
            using var connection = mySqlConnection;
            return (await connection.QueryAsync<Medico, Pessoa, Telefone, Medico>(
                sql,
                (medico, pessoa, telefone) =>
                {
                    if (!medicoDictionary.TryGetValue(medico.Id, out var medicoEntry))
                    {
                        medicoEntry = medico;
                        medicoEntry.Pessoa = pessoa;
                        medicoEntry.Telefones = new();
                        medicoDictionary.Add(medicoEntry.Id, medicoEntry);
                    }

                    if (telefone != null && !medicoEntry.Telefones.Any(t => t.Id == telefone.Id))
                        medicoEntry.Telefones.Add(telefone);

                    return medicoEntry;
                },
                parametros,
                splitOn: "Id"
                )).Distinct();
        }
        catch (Exception)
        {

            throw;
        }
    }
    #endregion
}
