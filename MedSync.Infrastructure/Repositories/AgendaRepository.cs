using Dapper;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using MedSync.Infrastructure.Repositories.Scripts;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

namespace MedSync.Infrastructure.Repositories;

public class AgendaRepository : BaseRepository, IAgendaRepository
{
    public AgendaRepository(MySqlConnection mySql, IHttpContextAccessor httpContextAccessor) : base(mySql, httpContextAccessor) { }


    public async Task<bool> CreateAsync(Agenda agenda)
    {
        var sql = AgendaScritps.Insert;

        return await GenericExecuteAsync(sql, agenda);
    }

    public async Task<IEnumerable<Agenda?>> GetAllAsync()
    {
        var sql = AgendaScritps.SelectBase;

        return await GetListAsync(sql, null);
    }

    public async Task<Agenda?> GetIdAsync(Guid id)
    {
        var sql = $"{AgendaScritps.SelectBase}{AgendaScritps.WhereId}";
        var parametro = new { Id = id };

        return (await GetListAsync(sql, parametro)).FirstOrDefault();
    }

    public async Task<IEnumerable<Agenda?>> GetMedicoIdAsync(Guid medicoId)
    {
        var sql = $"{AgendaScritps.SelectBase}{AgendaScritps.WhereMedicoId}";
        var parametro = new { MedicoId = medicoId };

        return await GetListAsync(sql, parametro);
    }

    public async Task<bool> UpdateAsync(Agenda agenda)
    {
        var sql = AgendaScritps.Update;

        return await GenericExecuteAsync(sql, agenda);
    }

    public bool AgendaPeriodoExiste(DateTime dataDisponivel, DayOfWeek dia, bool agendado)
    {
        var sql = AgendaScritps.DataHoraExiste;
        var parametros = new { DataDisponivel = dataDisponivel, DiaSemana = dia, Agendado = agendado };

        return JaExiste(sql, parametros);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var sql = AgendaScritps.Delete;
        var parametro = new { Id = id, ModificadoEm = DataHoraAtual() };

        return await GenericExecuteAsync(sql, parametro);
    }

    public bool Existe(Guid id)
    {
        var sql = AgendaScritps.Existe;
        var parametro = new { Id = id };

        return JaExiste(sql, parametro);
    }

    #region MÉTODOSPRIVADOS
    private async Task<IEnumerable<Agenda?>> GetListAsync(string sql, object? parametros)
    {
        try
        {
            var agendaDictionary = new Dictionary<Guid, Agenda>();

            CreateConnection(mySqlConnection);

           return (await mySqlConnection.QueryAsync<Agenda, Medico, Pessoa, Telefone, Horario, Agenda>(
           sql,
           (agenda, medico, pessoa, telefone, horario) =>
           {
               if (!agendaDictionary.TryGetValue(agenda.Id, out var agendaEntry))
               {
                   agendaEntry = agenda;
                   agendaEntry.Medico = medico;
                   agendaEntry.Medico.Pessoa = pessoa;

                   agendaEntry.Medico.Telefones = new();
                   agendaEntry.Horarios = new();

                   agendaDictionary.Add(agendaEntry.Id, agendaEntry);
               }

               if (telefone != null && telefone.MedicoId != null && !agendaEntry.Medico.Telefones.Exists(t => t.Id == telefone.Id))
                   agendaEntry.Medico.Telefones.Add(telefone);

               if (horario != null && !agendaEntry.Horarios.Exists(h => h.Id == horario.Id))
                   agendaEntry.Horarios.Add(horario);

               return agendaEntry;
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
