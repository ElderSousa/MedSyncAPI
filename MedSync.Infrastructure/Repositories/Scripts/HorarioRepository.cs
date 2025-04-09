using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

namespace MedSync.Infrastructure.Repositories.Scripts;

public class HorarioRepository : BaseRepository, IHorarioRepository
{
    public HorarioRepository(MySqlConnection mySql, IHttpContextAccessor httpContextAccessor) : base(mySql, httpContextAccessor) {}
    public async Task<bool> CreateAsync(Horario horario)
    {
        var sql = HorarioScripts.Insert;

        return await GenericExecuteAsync(sql, horario);
    }

    public async Task<IEnumerable<Horario?>> GetAllAsync()
    {
        var sql = HorarioScripts.SelectBase;

        return await GenericGetList<Horario>(sql, null);
    }

    public async Task<Horario?> GetIdAsync(Guid id)
    {
        var sql = $"{HorarioScripts.SelectBase}{HorarioScripts.WhereId}";
        var parametro = new {Id = id};

        return await GenericGetOne<Horario>(sql, parametro);
    }

    public async Task<IEnumerable<Horario?>> GetAgendaIdAsync(Guid agendaId)
    {
        var sql = $"{HorarioScripts.SelectBase}{HorarioScripts.WhereAgendaId}";
        var parametro = new {AgendaId = agendaId};

        return await GenericGetList<Horario>(sql, parametro);
    }

    public async Task<bool> UpdateAsync(Horario horario)
    {
        var sql = HorarioScripts.Update;

        return await GenericExecuteAsync(sql, horario);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var sql = HorarioScripts.Delete;
        var parametros = new {Id = id, ModificadoEm = DataHoraAtual()};

        return await GenericExecuteAsync(sql, parametros);
    }

    public bool Existe(Guid id)
    {
        var sql = HorarioScripts.Existe;
        var parametro = new {Id = id};

        return  JaExiste(sql, parametro);
    }

    public bool HorarioPeriodoExiste(TimeSpan horarioInicial, TimeSpan horarioFinal)
    {
        var sql = HorarioScripts.HorarioPeriodoExiste;
        var parametros = new {HorarioInicial = horarioInicial, HorarioFinal = horarioFinal};

        var resp =  JaExiste(sql, parametros);
        return resp;
    }
}
