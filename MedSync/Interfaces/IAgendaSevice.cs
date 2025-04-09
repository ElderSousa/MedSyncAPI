using MedSync.Application.Responses;
using static MedSync.Application.Requests.AgendaRequest;

namespace MedSync.Application.Interfaces;

public interface IAgendaSevice
{
    Task<Response> CreateAsync(AdicionarAgendaRequest agendamentoRequest);
    Task<IEnumerable<AgendaResponse?>> GetAllAsync();
    Task<AgendaResponse?> GetIdAsync(Guid id);
    Task<IEnumerable<AgendaResponse?>> GetMedicoIdAsync(Guid medicoId);
    Task<Response> UpdateAsync(AtualizarAgendaResquet agendamentoResquest);
    Task<Response> DeleteAsync(Guid id);
}
