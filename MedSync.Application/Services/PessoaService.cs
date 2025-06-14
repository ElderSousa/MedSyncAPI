﻿using AutoMapper;
using FluentValidation;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
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
        var pessoa = mapper.Map<Pessoa>(pessoaRequest);
        pessoa.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);
        pessoa.ValidacaoCadastrar = true;

        _response = await ExecultarValidacaoResponse(_pessoaValidation, pessoa);
        if (_response.Error)
            throw new ArgumentException(_response.Status);

        if (!await _pessoaRepository.CreateAsync(pessoa))
            throw new InvalidOperationException("Falha ao criar pessoa.");

        return ReturnResponseSuccess();
    }

    public async Task<PessoaResponse?> GetIdAsync(Guid id)
    {
        return mapper.Map<PessoaResponse>(await _pessoaRepository.GetIdAsync(id));
    }

    public async Task<PessoaResponse?> GetCPFAsync(string cpf)
    {
        return mapper.Map<PessoaResponse>(await _pessoaRepository.GetCPFAsync(cpf));
    }

    public async Task<Response> UpdateAsync(AtualizarPessoaRequest pessoaRequest)
    {
        var pessoa = mapper.Map<Pessoa>(pessoaRequest);
        pessoa.AdicionarBaseModel(null, DataHoraAtual(), false);
        pessoa.ValidacaoCadastrar = false;

        _response = await ExecultarValidacaoResponse(_pessoaValidation, pessoa);
        if (_response.Error)
            throw new ArgumentException(_response.Status);

        if (!await _pessoaRepository.UpdateAsync(pessoa))
            throw new InvalidOperationException("Falha na atualização em nossa base de dados.");

        return ReturnResponseSuccess();
    }


    public async Task<Response> DeleteAsync(Guid id)
    {
        if (!await _pessoaRepository.DeleteAsync(id))
            throw new InvalidOperationException("Falha ao realizar exclusão.");

        return ReturnResponseSuccess();
    }
}
