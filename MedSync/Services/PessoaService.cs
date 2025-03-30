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
            pessoa.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);

            _response = ExecultarValidacaoResponse(new PessoaValidation(_pessoaRepository, true), pessoa);
            if (_response.Error) 
                throw new ArgumentException(_response.Status);

            if (!await _pessoaRepository.CreateAsync(pessoa)) 
                throw new InvalidOperationException("Falha ao criar pessoa.");

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
            return mapper.Map<PessoaResponse>(await _pessoaRepository.GetIdAsync(id));  
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
            return mapper.Map<PessoaResponse>(await _pessoaRepository.GetCPFAsync(cpf));
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
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            if (!await _pessoaRepository.UpdateAsync(pessoaResponse))
                throw new InvalidOperationException("Falha na atualização em nossa base de dados.");
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
