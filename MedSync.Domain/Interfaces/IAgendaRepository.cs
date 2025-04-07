using MedSync.Domain.Entities;

namespace MedSync.Domain.Interfaces;

public interface IAgendaRepository
{
    Task<bool> CreateAsync(Agenda agenda);
    Task<IEnumerable<Agenda?>> GetAllAsync();
    Task<Agenda?> GetIdAsync(Guid id);
    Task<IEnumerable<Agenda?>> GetPacienteIdAsync(Guid pacienteId);
    Task<IEnumerable<Agenda?>> GetMedicoIdAsync(Guid medicoId);
    Task<bool> UpdateAsync(Agenda agenda);
    Task<bool> DeleteAsync(Guid id);
    bool Existe(Guid id);
    bool AgendamentoPeriodoExiste(DateTime dataHora);
}
