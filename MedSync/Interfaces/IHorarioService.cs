using MedSync.Application.Responses;
using static MedSync.Application.Requests.HorarioRequest;

namespace MedSync.Application.Interfaces;

public interface IHorarioService
{
    Task<Response> CreateAsync(AdicionarHorarioRequest horarioRequest);
    Task<IEnumerable<HorarioResponse?>> GetAllAsync();
    Task<IEnumerable<HorarioResponse?>> GetAgendadoFalseAsync();
    Task<HorarioResponse?> GetIdAsync(Guid id);
    Task<IEnumerable<HorarioResponse?>> GetAgendaIdAsync(Guid AgendaId);
    Task<Response> UpdateAsync(AtualizarHorarioRequest horarioResquest);
    Task<Response> UpdateStatusAsync(Guid id, bool agendado);
    Task<Response> DeleteAsync(Guid id);
}
