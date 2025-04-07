using MedSync.Domain.Entities;
using MedSync.Domain.Enum;

namespace MedSync.Domain.Interfaces;

public interface IAgendamentoRepository
{
    Task<bool> CreateAsync(Agendamento agendamento);
    Task<IEnumerable<Agendamento?>> GetAllAsync();
    Task<Agendamento?> GetIdAsync(Guid id);
    Task<IEnumerable<Agendamento?>> GetAgendaIdAsync(Guid agendaId);
    Task<IEnumerable<Agendamento?>> GetMedicoIdAsync(Guid medicoId);
    Task<bool> UpdateAsync(Agendamento agendamento);
    Task<bool> DeleteAsync(Guid id);
    bool Existe(Guid id);
    bool AgendamentoPeriodoExiste(DiaSemana dia, DateTime dataHora);
}
