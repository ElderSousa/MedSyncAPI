using MedSync.Application.Responses;
using MedSync.Domain.Entities;
using static MedSync.Application.Requests.PessoaRequest;
using static MedSync.Application.Responses.PessoaResponse;

namespace MedSync.Application.Interfaces;

public interface IPessoaService
{
    Task<Response> CreateAsync(AdicionarPessoaRequest pessoaRequest);
    Task<AdicionarPessoaResponse?> GetIdAsync(Guid id);
    Task<AdicionarPessoaResponse?> GetCPFAsync(string cpf);
    Task<Response> UpdateAsync(Pessoa pessoa);
    Task<Response> DeleteAsync(Guid id);
}
