using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using MedSync.Infrastructure.Repositories.Scripts;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

namespace MedSync.Infrastructure.Repositories;

public class TelefoneRepository : BaseRepository, ITelefoneRepository
{
    public TelefoneRepository(MySqlConnection mySql, IHttpContextAccessor httpContextAccessor) : base(mySql, httpContextAccessor) { }

    public async Task<bool> CreateAsync(Telefone telefone)
    {
        var sql = TelefoneScripts.Insert;

        return await GenericExecuteAsync(sql, telefone);
    }

    public async Task<IEnumerable<Telefone?>> GetAllAsync()
    {
        var sql = TelefoneScripts.SelectBase;
       
        return await GenericGetList<Telefone>(sql, null);
    }

    public async Task<Telefone?> GetIdAsync(Guid id)
    {
        var sql = $"{TelefoneScripts.SelectBase}{TelefoneScripts.WhereId}";
        var parametro = new { Id = id };

        return await GenericGetOne<Telefone>(sql, parametro);
    }
    public async Task<IEnumerable<Telefone?>> GetMedicoIdAsync(Guid medicoId)
    {
        var sql = $"{TelefoneScripts.SelectBase}{TelefoneScripts.WhereMedicoId}";
        var parametro = new { MedicoId = medicoId };

        return await GenericGetList<Telefone>(sql, parametro);
    }

    public async Task<IEnumerable<Telefone?>> GetPacienteIdAsync(Guid pacienteId)
    {
        var sql = $"{TelefoneScripts.SelectBase}{TelefoneScripts.WherePcienteId}";
        var parametro = new { PacienteId = pacienteId };

        return await GenericGetList<Telefone>(sql, parametro);
    }

    public async Task<Telefone?> GetNumeroAsync(string numero)
    {
        var sql = $"{TelefoneScripts.SelectBase}{TelefoneScripts.WhereNumero}";
        var parametro = new { Numero = numero };

        return await GenericGetOne<Telefone>(sql, parametro);
    }

    public async Task<bool> UpdateAsync(Telefone telefone)
    {
        var sql = TelefoneScripts.Update;

        return await GenericExecuteAsync(sql, telefone); ;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var sql = TelefoneScripts.Delete;
        var parametro = new { Id = id, ModificadoEm = DataHoraAtual() };

        return await GenericExecuteAsync(sql, parametro);
    }

    public bool Existe(Guid id)
    {
        var sql = TelefoneScripts.Existe;
        var parametro = new { Id = id};

        return JaExiste(sql, parametro);
    }

}
