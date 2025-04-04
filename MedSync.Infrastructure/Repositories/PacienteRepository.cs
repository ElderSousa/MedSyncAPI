using Dapper;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using MedSync.Infrastructure.Repositories.Scripts;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

namespace MedSync.Infrastructure.Repositories;

public class PacienteRepository : BaseRepository, IPacienteRepository
{
    public PacienteRepository(MySqlConnection mySql, IHttpContextAccessor httpContextAccessor) : base(mySql, httpContextAccessor) { }


    public async Task<bool> CreateAsync(Paciente paciente)
    {
        var sql = PacienteScripts.Insert;

        return await GenericExecuteAsync(sql, paciente);
    }

    public async Task<IEnumerable<Paciente?>> GetAllAsync()
    {
        var sql = PacienteScripts.SelectBase;

        return await GetListAsync(sql, null);
    }

    public async Task<Paciente?> GetIdAsync(Guid id)
    {
        var sql = $"{PacienteScripts.SelectBase}{PacienteScripts.WhereId}";
        var parametro = new { Id = id };

        return (await GetListAsync(sql, parametro)).FirstOrDefault();
    }

    public async Task<bool> UpdateAsync(Paciente paciente)
    {
        var sql = PacienteScripts.Update;

        return await GenericExecuteAsync(sql, paciente);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var sql = PacienteScripts.Delete;
        var parametros = new { Id = id, ModificadoEm = DataHoraAtual() };

        return await GenericExecuteAsync(sql, parametros);
    }

    public bool Existe(Guid id)
    {
        var sql = PacienteScripts.Existe;
        var parametro = new { Id = id };

        return JaExiste(sql, parametro);
    }

    #region MÉTODOSPRIVADOS
    private async Task<IEnumerable<Paciente>> GetListAsync(string sql, object? parametros)
    {
        var pacienteDictionary = new Dictionary<Guid, Paciente>();
        try
        {
            CreateConnection(mySqlConnection);

            return (await mySqlConnection.QueryAsync<Paciente, Pessoa, Endereco, Telefone, Paciente>(
                sql,
                (paciente, pessoa, endereco, telefone) =>
                {
                    if (!pacienteDictionary.TryGetValue(paciente.Id, out var pacienteEntry))
                    {
                        pacienteEntry = paciente;
                        pacienteEntry.Pessoa = pessoa;
                        pacienteEntry.Endereco = endereco;
                        pacienteDictionary.Add(pacienteEntry.Id, pacienteEntry);
                    }

                    if (telefone != null && !pacienteEntry.Telefones.Any(t => t.Id == telefone.Id))
                        pacienteEntry.Telefones.Add(telefone);

                    return pacienteEntry;
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
