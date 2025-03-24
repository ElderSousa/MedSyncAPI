using System.Data.Common;
using AutoMapper;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using MedSync.Application.Validation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using static MedSync.Application.Requests.PessoaRequest;

namespace MedSync.Application.Services;

public class PessoaService : BaseService, IPessoaService
{
    private Response _response = new();
    private readonly IPessoaRepository _pessoaRepository;
    public PessoaService(IPessoaRepository pessoaRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(mapper, httpContextAccessor)
    {
        _pessoaRepository = pessoaRepository;
    }

    public async Task<Response> CreateAsync(AdicionarPessoaRequest pessoaRequest)
    {
        try
        {
            var pessoa = mapper.Map<Pessoa>(pessoaRequest);
            pessoa.AdicionarBaseModel(null, DataHoraAtual(), true);

            _response = ExecultarValidacaoResponse(new PessoaValidation(_pessoaRepository, true), pessoa);
            if (_response.Error) 
                return _response;

            if (!await _pessoaRepository.CreateAsync(pessoa)) 
                return ReturnResponse("Pessao não adicionada ", true);

        }
        catch (DbException ex)
        {
            return ReturnResponse(ex.Message, true);
        }
        catch (AutoMapperMappingException ex)
        {
            return ReturnResponse(ex.Message, true);
        }

        return ReturnResponseSuccess();
    }
}
