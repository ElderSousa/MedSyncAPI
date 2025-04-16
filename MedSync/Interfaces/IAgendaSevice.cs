using MedSync.Application.PaginationModel;
using MedSync.Application.Responses;
using static MedSync.Application.Requests.AgendaRequest;

namespace MedSync.Application.Interfaces;

public interface IAgendaSevice
{
    Task<Response> CreateAsync(AdicionarAgendaRequest agendamentoRequest);
    Task<Pagination<AgendaResponse>> GetAllAsync(int page, int pageSize);
    Task<AgendaResponse?> GetIdAsync(Guid id);
    Task<Pagination<AgendaResponse>> GetMedicoIdAsync(Guid medicoId, int page, int pageSize);
    Task<Response> UpdateAsync(AtualizarAgendaResquet agendamentoResquest);
    Task<Response> DeleteAsync(Guid id);
}
