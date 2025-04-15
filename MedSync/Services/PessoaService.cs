using AutoMapper;
using FluentValidation;
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
    private readonly IValidator<Pessoa> _pessoaValidation;
    public PessoaService(IPessoaRepository pessoaRepository,
        IValidator<Pessoa> pessoaValidation,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<PessoaService> logger) : base(mapper, httpContextAccessor, logger)
    {
        _pessoaRepository = pessoaRepository;
        _pessoaValidation = pessoaValidation;
    }

    public async Task<Response> CreateAsync(AdicionarPessoaRequest pessoaRequest)
    {
        try
        {
            var pessoa = mapper.Map<Pessoa>(pessoaRequest);
            pessoa.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);
            pessoa.ValidacaoCadastrar = true;

            _response = await ExecultarValidacaoResponse(_pessoaValidation, pessoa);
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
            var pessoa = mapper.Map<Pessoa>(pessoaRequest);
            pessoa.AdicionarBaseModel(null, DataHoraAtual(), false);
            pessoa.ValidacaoCadastrar = false;

            _response = await ExecultarValidacaoResponse(_pessoaValidation, pessoa);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            if (!await _pessoaRepository.UpdateAsync(pessoa))
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
