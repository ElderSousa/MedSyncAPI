using MedSync.Application.Responses;
using static MedSync.Application.Requests.HorarioRequest;

namespace MedSync.Application.Interfaces;

public interface IHorarioService
{
    Task<Response> CreateAsync(AdicionarHorarioRequest horarioRequest);
    Task<IEnumerable<HorarioResponse?>> GetAllAsync();
    Task<HorarioResponse?> GetIdAsync(Guid id);
    Task<IEnumerable<HorarioResponse?>> GetAgendaIdAsync(Guid AgendaId);
    Task<Response> UpdateAsync(AtualizarHorarioRequest horarioResquest);
    Task<Response> DeleteAsync(Guid id);
}
