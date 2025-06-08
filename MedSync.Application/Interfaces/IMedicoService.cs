using MedSync.Application.PaginationModel;
using MedSync.Application.Responses;
using static MedSync.Application.Requests.MedicoResquest;

namespace MedSync.Application.Interfaces;

public interface IMedicoService
{
    Task<Response> CreateAsync(AdicionarMedicoRequest medicoRequest);
    Task<MedicoResponse?> GetIdAsync(Guid id);
    Task<Pagination<MedicoResponse>> GetAllAsync(int page, int pageSize);
    Task<MedicoResponse?> GetCRMAsync(string crm);
    Task<Response> UpdateAsync(AtualizarMedicoRequest medicoRequest);
    Task<Response> DeleteAsync(Guid id);
}
