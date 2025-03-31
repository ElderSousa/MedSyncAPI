using MedSync.Application.Responses;
using static MedSync.Application.Requests.PacienteRequest;

namespace MedSync.Application.Interfaces;

public interface IPacienteService
{
    Task<Response> CreateAsync(AdicionarPacienteRequest paciente);
    Task<PacienteResponse?> GetIdAsync(Guid id);
    Task<IEnumerable<PacienteResponse?>> GetAllAsync();
    Task<Response> UpdateAsync(AtualizarPacienteRequest paciente);
    Task<Response> DeleteAsync(Guid id);
}
