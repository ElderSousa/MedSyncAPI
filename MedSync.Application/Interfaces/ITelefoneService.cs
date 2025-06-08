using MedSync.Application.PaginationModel;
using MedSync.Application.Responses;
using static MedSync.Application.Requests.TelefoneRequest;

namespace MedSync.Application.Interfaces;

public interface ITelefoneService
{
    Task<Response> CreateAsync(AdicionarTelefoneRequest telefoneRequest);
    Task<Pagination<TelefoneResponse>> GetAllAsync(int page, int pageSize);
    Task<TelefoneResponse?> GetIdAsync(Guid id);
    Task<Pagination<TelefoneResponse>> GetMedicoIdAsync(Guid medicoId, int page, int pageSize);
    Task<Pagination<TelefoneResponse>> GetPacienteIdAsync(Guid pacienteId, int page, int pageSize);
    Task<TelefoneResponse?> GetNumeroAsync(string numero);
    Task<Response> UpdateAsync(AtualizarTelefoneRequest telefoneRequest);
    Task<Response> DeleteAsync(Guid id);
}
