﻿using AutoMapper;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using MedSync.Application.Validation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static MedSync.Application.Requests.TelefoneRequest;

namespace MedSync.Application.Services;

public class TelefoneService : BaseService, ITelefoneService
{
    private Response _response = new();
    private readonly ITelefoneRepository _telefoneRepository;
    public TelefoneService(ITelefoneRepository telefoneRepository,
        IMapper mapper, IHttpContextAccessor httpContextAccessor,
        ILogger<TelefoneService> logger) : base(mapper, httpContextAccessor, logger)
    {
        _telefoneRepository = telefoneRepository;
    }

    public async Task<Response> CreateAsync(AdicionarTelefoneRequest telefoneRequest)
    {
        try
        {
            var telefone = mapper.Map<Telefone>(telefoneRequest);
            telefone.AdicionarBaseModel(null, DataHoraAtual(), true);

            _response = ExecultarValidacaoResponse(new TelefoneValidation(_telefoneRepository, true), telefone);
            if (_response.Error) return _response;

            if (!await _telefoneRepository.CreateAsync(telefone))
                ReturnResponse("Telefone não adicionado em nossa base de dados.", true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "CreateAsync");
            throw;
        }

        return ReturnResponseSuccess();
    }

    public async Task<TelefoneResponse?> GetIdAsync(Guid id)
    {
        try
        {
            return mapper.Map<TelefoneResponse>(await _telefoneRepository.GetIdAsync(id));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetIdAsync");
            throw;
        }
    }

    public async Task<TelefoneResponse?> GetNumeroAsync(string numero)
    {
        try
        {
            return mapper.Map<TelefoneResponse>(await _telefoneRepository.GetNumeroAsync(numero));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetNumeroAsync");
            throw;
        }
    }

    public async Task<Response> UpdateAsync(AtualizarTelefoneRequest telefoneRequest)
    {
        try
        {
            var telefone = mapper.Map<Telefone>(telefoneRequest);
            telefone.AdicionarBaseModel(null, DataHoraAtual(), false);

            _response = ExecultarValidacaoResponse(new TelefoneValidation(_telefoneRepository, false), telefone);
            if (_response.Error) return _response;

            if (!await _telefoneRepository.UpdateAsync(telefone)) 
                return ReturnResponse("Telefone não atualizado.", true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "UpdateAsync");
            throw;
        }

        return ReturnResponseSuccess();
    }
    public async Task<Response> DeleteAsync(Guid id)
    {
        try
        {
            if (await _telefoneRepository.DeleteAsync(id))
                return ReturnResponse("Telefone não excluído da nossa base de dados.", true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "DeleteAsync");
            throw;
        }

        return ReturnResponseSuccess();
    }
}
