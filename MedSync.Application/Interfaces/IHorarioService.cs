using MedSync.Application.PaginationModel;
using MedSync.Application.Responses;
using static MedSync.Application.Requests.HorarioRequest;

namespace MedSync.Application.Interfaces;

public interface IHorarioService
{
    Task<Response> CreateAsync(AdicionarHorarioRequest horarioRequest);
    Task<Pagination<HorarioResponse>> GetAllAsync(int page, int pageSize);
    Task<Pagination<HorarioResponse>> GetAgendadoFalseAsync(int page, int pageSize);
    Task<HorarioResponse?> GetIdAsync(Guid id);
    Task<Pagination<HorarioResponse>> GetAgendaIdAsync(Guid AgendaId, int page, int pageSize);
    Task<Response> UpdateAsync(AtualizarHorarioRequest horarioResquest);
    Task<Response> UpdateStatusAsync(Guid id, bool agendado);
    Task<Response> DeleteAsync(Guid id);
}
