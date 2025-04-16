using MedSync.Application.PaginationModel;
using MedSync.Application.Responses;
using static MedSync.Application.Requests.PacienteRequest;

namespace MedSync.Application.Interfaces;

public interface IPacienteService
{
    Task<Response> CreateAsync(AdicionarPacienteRequest paciente);
    Task<PacienteResponse?> GetIdAsync(Guid id);
    Task<Pagination<PacienteResponse>> GetAllAsync(int page, int pageSize);
    Task<Response> UpdateAsync(AtualizarPacienteRequest paciente);
    Task<Response> DeleteAsync(Guid id);
}
