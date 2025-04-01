using Dapper;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using MedSync.Infrastructure.Repositories.Scripts;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

namespace MedSync.Infrastructure.Repositories;

public class AgendamentoRepository : BaseRepository, IAgendamentoRepository
{
    public AgendamentoRepository(MySqlConnection mySql, IHttpContextAccessor httpContextAccessor) : base(mySql, httpContextAccessor) { }


    public async Task<bool> CreateAsync(Agendamento agendamento)
    {
        var sql = AgendamentoScritps.Insert;

        return await GenericExecuteAsync(sql, agendamento);
    }

    public async Task<IEnumerable<Agendamento?>> GetAllAsync()
    {
        var sql = AgendamentoScritps.SelectBase;

        return await GetListAsync(sql, null);
    }

    public async Task<Agendamento?> GetIdAsync(Guid id)
    {
        var sql = $"{AgendamentoScritps.SelectBase}{AgendamentoScritps.WhereId}";
        var parametro = new { Id = id };

        return (await GetListAsync(sql, parametro)).FirstOrDefault();
    }

    public async Task<IEnumerable<Agendamento?>> GetMedicoIdAsync(Guid medicoId)
    {
        var sql = $"{AgendamentoScritps.SelectBase}{AgendamentoScritps.WhereMedicoId}";
        var parametro = new { MedicoId = medicoId };

        return await GetListAsync(sql, parametro);
    }

    public async Task<IEnumerable<Agendamento?>> GetPacienteIdAsync(Guid pacienteId)
    {
        var sql = $"{AgendamentoScritps.SelectBase}{AgendamentoScritps.WherePacienteId}";
        var parametro = new { PacienteId = pacienteId };

        return await GetListAsync(sql, parametro);
    }

    public async Task<bool> UpdateAsync(Agendamento agendamento)
    {
        var sql = AgendamentoScritps.Update;

        return await GenericExecuteAsync(sql, agendamento);
    }

    public bool AgendamentoPeriodoExiste(DateTime agendadoPara)
    {
        var sql = AgendamentoScritps.AgendadoParaExiste;
        var parametro = new { AgendadoPara = agendadoPara };

        return JaExiste(sql, parametro);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var sql = AgendamentoScritps.Delete;
        var parametro = new { Id = id, ModificadoEm = DataHoraAtual() };

        return await GenericExecuteAsync(sql, parametro);
    }

    public bool Existe(Guid id)
    {
        var sql = AgendamentoScritps.Existe;
        var parametro = new { Id = id };

        return JaExiste(sql, parametro);
    }



    #region MÉTODOSPRIVADOS
    private async Task<IEnumerable<Agendamento?>> GetListAsync(string sql, object? parametros)
    {
        try
        {
            var agendamentoDictionary = new Dictionary<Guid, Agendamento>();

            using (var connection = mySqlConnection)
            {
                return (await connection.QueryAsync<Agendamento, Paciente, Medico, Pessoa, Pessoa, Telefone, Endereco, Agendamento>(
               sql,
               (agendamento, paciente, medico, pessoaPaciente, pessoaMedico, telefone, endereco) =>
               {
                   if (!agendamentoDictionary.TryGetValue(agendamento.Id, out var agendamentoEntry))
                   {
                       agendamentoEntry = agendamento;
                       agendamentoEntry.Paciente = paciente;
                       agendamentoEntry.Paciente.Pessoa = pessoaPaciente;
                       agendamentoEntry.Medico = medico;
                       agendamentoEntry.Medico.Pessoa = pessoaMedico;

                       agendamentoEntry.Paciente.Telefones = new();
                       agendamentoEntry.Medico.Telefones = new();

                       agendamentoDictionary.Add(agendamentoEntry.Id, agendamentoEntry);
                   }

                   if (telefone != null && telefone.MedicoId != null && !agendamentoEntry.Medico.Telefones.Any(t => t.Id == telefone.Id))
                       agendamentoEntry.Medico.Telefones.Add(telefone);
                   else
                       agendamentoEntry.Paciente.Telefones.Add(telefone!);

                   agendamentoEntry.Paciente.Endereco = endereco;

                   return agendamentoEntry;
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
