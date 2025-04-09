using MedSync.Domain.Entities;

namespace MedSync.Domain.Interfaces;

public interface IHorarioRepository
{
    Task<bool> CreateAsync(Horario horario);
    Task<IEnumerable<Horario?>> GetAllAsync();
    Task<Horario?> GetIdAsync(Guid id);
    Task<IEnumerable<Horario?>> GetAgendaIdAsync(Guid AgendaId);
    Task<bool> UpdateAsync(Horario horario);
    Task<bool> DeleteAsync(Guid id);
    bool Existe(Guid id);
    bool HorarioPeriodoExiste(TimeSpan horarioInicial, TimeSpan horarioFinal);
}
