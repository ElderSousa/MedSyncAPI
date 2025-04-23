using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using MedSync.Infrastructure.Repositories.Scripts;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

namespace MedSync.Infrastructure.Repositories;

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

    public async Task<IEnumerable<Horario?>> GetAgendadoFalseAsync()
    {
        var sql = $"{HorarioScripts.SelectBase}{HorarioScripts.WhereAgendadoFalse}";
        
        return await GenericGetList<Horario>(sql, null);
    }

    public async Task<bool> UpdateAsync(Horario hora)
    {
        var sql = HorarioScripts.Update;

        return await GenericExecuteAsync(sql, hora);
    }

    public async Task<bool> UpdateStatusAsync(Guid id, bool agendado)
    {
        var sql = HorarioScripts.Status;
        var parametros = new {Id = id, Agendado = agendado, ModificadoEm = DataHoraAtual()};

        return await GenericExecuteAsync(sql, parametros);
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

    public bool HorarioExiste(TimeSpan hora, bool agendado)
    {
        var sql = HorarioScripts.HorarioExiste;
        var parametros = new {Hora = hora, Agendado = agendado};

        return JaExiste(sql, parametros);
    }

    public bool ExisteIntervalo(TimeSpan hora)
    {
        var sql = HorarioScripts.WhereHoraIntervaloInvalido;
        var parametros = new { Hora = hora.ToString()};

        return JaExiste(sql, parametros);
    }
}
