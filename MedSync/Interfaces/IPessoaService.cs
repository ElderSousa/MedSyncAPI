using MedSync.Application.Responses;
using MedSync.Domain.Entities;
using static MedSync.Application.Requests.PessoaRequest;

namespace MedSync.Application.Interfaces;

public interface IPessoaService
{
    Task<Response> CreateAsync(AdicionarPessoaRequest pessoaRequest);
    Task<PessoaResponse?> GetIdAsync(Guid id);
    Task<PessoaResponse?> GetCPFAsync(string cpf);
    Task<Response> UpdateAsync(AtualizarPessoaRequest pessoa);
    Task<Response> DeleteAsync(Guid id);
}
