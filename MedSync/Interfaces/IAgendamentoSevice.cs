using MedSync.Application.Responses;
using static MedSync.Application.Requests.AgendamentoRequest;

namespace MedSync.Application.Interfaces;

public interface IAgendamentoSevice
{
    Task<Response> CreateAsync(AdicionarAgendamentoRequest agendamentoRequest);
    Task<IEnumerable<AgendamentoResponse?>> GetAllAsync();
    Task<AgendamentoResponse?> GetIdAsync(Guid id);
    Task<IEnumerable<AgendamentoResponse?>> GetPacienteIdAsync(Guid pacienteId);
    Task<IEnumerable<AgendamentoResponse?>> GetMedicoIdAsync(Guid medicoId);
    Task<Response> UpdateAsync(AtualizarAgendamentoResquet agendamentoResquest);
    Task<Response> DeleteAsync(Guid id);
}
