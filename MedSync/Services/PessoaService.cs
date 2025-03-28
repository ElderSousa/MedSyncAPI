using AutoMapper;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using MedSync.Application.Validation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static MedSync.Application.Requests.PessoaRequest;

namespace MedSync.Application.Services;

public class PessoaService : BaseService, IPessoaService
{
    private Response _response = new();
    private readonly IPessoaRepository _pessoaRepository;
    public PessoaService(IPessoaRepository pessoaRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<PessoaService> logger) : base(mapper, httpContextAccessor, logger)
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
        catch (Exception ex)
        {
            logger.LogError(ex, "CreateAsync");
            throw;
        }
     

        return ReturnResponseSuccess();
    }

    public async Task<PessoaResponse?> GetIdAsync(Guid id)
    {
        try
        {
            var pessoa = await _pessoaRepository.GetIdAsync(id);

            return mapper.Map<PessoaResponse>(pessoa);
           
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetIdAsync");
            throw;
        }
    }

    public async Task<PessoaResponse?> GetCPFAsync(string cpf)
    {
        try
        {
            var pessoa = await _pessoaRepository.GetCPFAsync(cpf);
            return mapper.Map<PessoaResponse>(pessoa);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetIdAsync");
            throw;
        }
    }

    public async Task<Response> UpdateAsync(AtualizarPessoaRequest pessoaRequest)
    {
        try
        {
            var pessoaResponse = mapper.Map<Pessoa>(pessoaRequest);
            pessoaResponse.AdicionarBaseModel(null, DataHoraAtual(), false);

            _response = ExecultarValidacaoResponse(new PessoaValidation(_pessoaRepository, false), pessoaResponse);
            if (_response.Error) return _response;

            if (!await _pessoaRepository.UpdateAsync(pessoaResponse))
                return ReturnResponse("Atualização não obteve sucesso.", true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetIdAsync");
            throw;
        }

        return ReturnResponseSuccess();
    }
    

    public async Task<Response> DeleteAsync(Guid id)
    {
        try
        {
            if (!await _pessoaRepository.DeleteAsync(id))
                return ReturnResponse("Exclusão não realizada.", true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetIdAsync");
            throw;
        }

        return ReturnResponseSuccess();
    }
}
