using MedSync.Application.Responses;
using static MedSync.Application.Requests.EnderecoRequest;

namespace MedSync.Application.Interfaces;

public interface IEnderecoService
{
    Task<Response> CreateAsync(AdicionarEnderecoRequest enderecoRequest);
    Task<EnderecoResponse?> GetIdAsync(Guid id);
    Task<EnderecoResponse?> GetCEPAsync(string cep);
    Task<Response> UpdateAsync(AtualizarEnderecoRequest enderecoRequest);
    Task<Response> DeleteAsync(Guid id);
}
