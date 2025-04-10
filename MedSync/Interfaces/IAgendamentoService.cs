using MedSync.Application.Responses;
using static MedSync.Application.Requests.AgendamentoRequest;

namespace MedSync.Application.Interfaces;

public interface IAgendamentoService
{
    Task<Response> CreateAsync(AdicionaAgendamentoRequest agendamentoResquest);
    Task<IEnumerable<AgendamentoResponse?>> GetAllAsync();
    Task<AgendamentoResponse?> GetIdAsync(Guid id);
    Task<IEnumerable<AgendamentoResponse?>> GetAgendaIdAsync(Guid agendaId);
    Task<IEnumerable<AgendamentoResponse?>> GetMedicoIdAsync(Guid medicoId);
    Task<IEnumerable<AgendamentoResponse?>> GetPacienteIdAsync(Guid pacienteId);
    Task<Response> UpdateAsync(AtualizarAgendamentoRequest agendamentoRequest);
    Task<Response> DeleteAsync(Guid id);
}
