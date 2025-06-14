﻿using AutoMapper;
using FluentValidation;
using MedSync.Application.Interfaces;
using MedSync.Application.PaginationModel;
using MedSync.Application.Responses;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static MedSync.Application.Requests.PacienteRequest;

namespace MedSync.Application.Services;

public class PacienteService : BaseService, IPacienteService
{
    private Response _response = new();
    private readonly IPacienteRepository _pacienteRepository;
    private readonly IPessoaService _pessoaService;
    private readonly IEnderecoService _enderecoService;
    private readonly ITelefoneService _telefoneService;
    private readonly IValidator<Paciente> _pacienteValidator;
    public PacienteService(IPacienteRepository pacienteRepository,
        IPessoaService pessoaService,
        IEnderecoService enderecoService,
        ITelefoneService telefoneService,
        IValidator<Paciente> pacienteValidator,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<PacienteService> logger) : base(mapper, httpContextAccessor, logger)
    {
        _pacienteRepository = pacienteRepository;
        _pessoaService = pessoaService;
        _enderecoService = enderecoService;
        _telefoneService = telefoneService;
        _pacienteValidator = pacienteValidator;
    }

    public async Task<Response> CreateAsync(AdicionarPacienteRequest pacienteRequest)
    {
        var paciente = mapper.Map<Paciente>(pacienteRequest);
        paciente.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);
        paciente.ValidacaoCadastrar = true;

        _response = await ExecultarValidacaoResponse(_pacienteValidator, paciente);
        if (_response.Error)
            throw new ArgumentException(_response.Status);

        var pessoa = await _pessoaService.GetCPFAsync(pacienteRequest.Pessoa.CPF!);
        if (pessoa == null || pessoa.Id == Guid.Empty)
        {
            await _pessoaService.CreateAsync(pacienteRequest.Pessoa);
            pessoa = await _pessoaService.GetCPFAsync(pacienteRequest.Pessoa.CPF!);
            paciente.PessoaId = pessoa!.Id;
        }

        paciente.PessoaId = pessoa.Id;
        if (!await _pacienteRepository.CreateAsync(paciente))
            throw new InvalidOperationException("Falha ao adicionar paciente em nossa base de dados.");

        pacienteRequest.Endereco.PacienteId = paciente.Id;
        await _enderecoService.CreateAsync(pacienteRequest.Endereco);

        foreach (var telefone in pacienteRequest.Telefones)
        {
            telefone.PacienteId = paciente.Id;
            await _telefoneService.CreateAsync(telefone);
            paciente.Telefones.Add(mapper.Map<Telefone>(telefone));
        }

        return ReturnResponseSuccess();
    }
    public async Task<Pagination<PacienteResponse>> GetAllAsync(int page, int pageSize)
    {
        var pacientes = mapper.Map<IEnumerable<PacienteResponse>>(await _pacienteRepository.GetAllAsync());

        return Paginar(pacientes, page, pageSize);
    }

    public async Task<PacienteResponse?> GetIdAsync(Guid id)
    {
        return mapper.Map<PacienteResponse>(await _pacienteRepository.GetIdAsync(id));
    }

    public async Task<Response> UpdateAsync(AtualizarPacienteRequest pacienteRequest)
    {
        var paciente = mapper.Map<Paciente>(pacienteRequest);
        paciente.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), false);
        paciente.ValidacaoCadastrar = false;

        _response = await ExecultarValidacaoResponse(_pacienteValidator, paciente);
        if (_response.Error)
            throw new ArgumentException(_response.Status);

        await _pessoaService.UpdateAsync(pacienteRequest.Pessoa);

        await _enderecoService.UpdateAsync(pacienteRequest.Endereco);

        foreach (var telefone in pacienteRequest.Telefones)
            await _telefoneService.UpdateAsync(telefone);

        return ReturnResponseSuccess();
    }

    public async Task<Response> DeleteAsync(Guid id)
    {
        var deletado = await _pacienteRepository.DeleteAsync(id);
        if (!deletado)
            throw new InvalidOperationException("Falha na exclusão da nossa base de dados.");

        return ReturnResponseSuccess();
    }
}

