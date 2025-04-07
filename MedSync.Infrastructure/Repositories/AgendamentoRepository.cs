using MedSync.Domain.Entities;
using MedSync.Domain.Enum;
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
        var sql = AgendamentoScripts.Insert;

        return await GenericExecuteAsync(sql, agendamento);
    }

    public async Task<IEnumerable<Agendamento?>> GetAllAsync()
    {
        var sql = AgendamentoScripts.SelectBase;

        return await GenericGetList<Agendamento>(sql, null);
    }

    public async Task<Agendamento?> GetIdAsync(Guid id)
    {
        var sql = $"{AgendamentoScripts.SelectBase}{AgendamentoScripts.WhereId}";
        var parametro = new { Id = id };

        return await GenericGetOne<Agendamento>(sql, parametro);
    }

    public async Task<IEnumerable<Agendamento?>> GetMedicoIdAsync(Guid medicoId)
    {
        var sql = $"{AgendamentoScripts.SelectBase}{AgendamentoScripts.WhereMedicoId}";
        var parametro = new { MedicoId = medicoId };

        return await GenericGetList<Agendamento>(sql, parametro);
    }

    public async Task<IEnumerable<Agendamento?>> GetAgendaIdAsync(Guid agendaId)
    {
        var sql = $"{AgendamentoScripts.SelectBase}{AgendamentoScripts.WhereAgendaId}";
        var parametro = new { AgendaId = agendaId };

        return await GenericGetList<Agendamento>(sql, parametro);
    }

    public async Task<bool> UpdateAsync(Agendamento agendamento)
    {
        var sql = AgendamentoScripts.Update;

        return await GenericExecuteAsync(sql, agendamento);
    }

    public bool AgendamentoPeriodoExiste(DiaSemana dia, DateTime dataHora)
    {
        var sql = AgendamentoScripts.periodo;
        var parametros = new { DiaSemana = dia, DataHora = dataHora};

        return JaExiste(sql, parametros);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var sql =   AgendamentoScripts.Delete;
        var parametro = new { Id = id, ModificadoEm = DataHoraAtual() };

        return await GenericExecuteAsync(sql, parametro);
    }

    public bool Existe(Guid id)
    {
        var sql = AgendamentoScripts.Existe;
        var parametro = new { Id = id };

        return JaExiste(sql, parametro);
    }

}
