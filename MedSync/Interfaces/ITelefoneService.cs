using MedSync.Application.Responses;
using static MedSync.Application.Requests.TelefoneRequest;

namespace MedSync.Application.Interfaces;

public interface ITelefoneService
{
    Task<Response> CreateAsync(AdicionarTelefoneRequest telefoneRequest);
    Task<TelefoneResponse?> GetIdAsync(Guid id);
    Task<TelefoneResponse?> GetNumeroAsync(string numero);
    Task<Response> UpdateAsync(AtualizarTelefoneRequest telefoneRequest);
    Task<Response> DeleteAsync(Guid id);
}
