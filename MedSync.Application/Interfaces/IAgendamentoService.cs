using MedSync.Application.PaginationModel;
using MedSync.Application.Responses;
using static MedSync.Application.Requests.AgendamentoRequest;

namespace MedSync.Application.Interfaces;

public interface IAgendamentoService
{
    Task<Response> CreateAsync(AdicionaAgendamentoRequest agendamentoResquest);
    Task<Pagination<AgendamentoResponse>> GetAllAsync(int page, int pageSize);
    Task<AgendamentoResponse?> GetIdAsync(Guid id);
    Task<Pagination<AgendamentoResponse>> GetAgendaIdAsync(Guid agendaIdint,int page, int pageSize);
    Task<Pagination<AgendamentoResponse>> GetMedicoIdAsync(Guid medicoId, int page, int pageSize);
    Task<Pagination<AgendamentoResponse>> GetPacienteIdAsync(Guid pacienteId, int page, int pageSize);
    Task<Response> UpdateAsync(AtualizarAgendamentoRequest agendamentoRequest);
    Task<Response> DeleteAsync(Guid id);
}
