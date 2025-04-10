using MedSync.Domain.Entities;

namespace MedSync.Domain.Interfaces;

public interface IHorarioRepository
{
    Task<bool> CreateAsync(Horario hor);
    Task<IEnumerable<Horario?>> GetAllAsync();
    Task<IEnumerable<Horario?>> GetAgendadoFalseAsync();
    Task<Horario?> GetIdAsync(Guid id);
    Task<IEnumerable<Horario?>> GetAgendaIdAsync(Guid AgendaId);
    Task<bool> UpdateAsync(Horario horario);
    Task<bool> UpdateStatusAsync(Guid id, bool agendado);
    Task<bool> DeleteAsync(Guid id);
    bool Existe(Guid id);
    bool HorarioExiste(TimeSpan hora, bool Agendado);
}
