using MedSync.Application.Requests;
using MedSync.Application.Responses;
using static MedSync.Application.Requests.PessoaRequest;

namespace MedSync.Application.Interfaces;

public interface IPessoaService
{
    Task<Response> CreateAsync(AdicionarPessoaRequest pessoaRequest);
}
