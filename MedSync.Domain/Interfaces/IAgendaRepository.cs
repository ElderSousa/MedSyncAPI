using MedSync.Domain.Entities;

namespace MedSync.Domain.Interfaces;

public interface IAgendaRepository
{
    Task<bool> CreateAsync(Agenda agenda);
    Task<IEnumerable<Agenda?>> GetAllAsync();
    Task<Agenda?> GetIdAsync(Guid id);
    Task<IEnumerable<Agenda?>> GetMedicoIdAsync(Guid medicoId);
    Task<bool> UpdateAsync(Agenda agenda);
    Task<bool> DeleteAsync(Guid id);
    bool Existe(Guid id);
    bool AgendaPeriodoExiste(DateTime dataDisponivel, DayOfWeek dia, bool agendado);
}
