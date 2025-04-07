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


    public async Task<bool> CreateAsync(Agenda agendamento)
    {
        var sql = AgendaScritps.Insert;

        return await GenericExecuteAsync(sql, agendamento);
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

    public async Task<IEnumerable<Agenda?>> GetPacienteIdAsync(Guid pacienteId)
    {
        var sql = $"{AgendaScritps.SelectBase}{AgendaScritps.WherePacienteId}";
        var parametro = new { PacienteId = pacienteId };

        return await GetListAsync(sql, parametro);
    }

    public async Task<bool> UpdateAsync(Agenda agendamento)
    {
        var sql = AgendaScritps.Update;

        return await GenericExecuteAsync(sql, agendamento);
    }

    public bool AgendamentoPeriodoExiste(DateTime agendadoPara)
    {
        var sql = AgendaScritps.AgendadoParaExiste;
        var parametro = new { AgendadoPara = agendadoPara };

        return JaExiste(sql, parametro);
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

            using (var connection = mySqlConnection)
            {
                return (await connection.QueryAsync<Agenda, Paciente, Medico, Pessoa, Pessoa, Telefone, Endereco, Agenda>(
               sql,
               (agenda, paciente, medico, pessoaPaciente, pessoaMedico, telefone, endereco) =>
               {
                   if (!agendaDictionary.TryGetValue(agenda.Id, out var agendaEntry))
                   {
                       agendaEntry = agenda;
                       agendaEntry.Paciente = paciente;
                       agendaEntry.Paciente.Pessoa = pessoaPaciente;
                       agendaEntry.Medico = medico;
                       agendaEntry.Medico.Pessoa = pessoaMedico;

                       agendaEntry.Paciente.Telefones = new();
                       agendaEntry.Medico.Telefones = new();

                       agendaDictionary.Add(agendaEntry.Id, agendaEntry);
                   }

                   if (telefone != null && telefone.MedicoId != null && !agendaEntry.Medico.Telefones.Any(t => t.Id == telefone.Id))
                       agendaEntry.Medico.Telefones.Add(telefone);
                   else
                       agendaEntry.Paciente.Telefones.Add(telefone!);

                   agendaEntry.Paciente.Endereco = endereco;

                   return agendaEntry;
               },
               parametros,
               splitOn: "Id"
               )).Distinct();
            }
           
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    #endregion
}
