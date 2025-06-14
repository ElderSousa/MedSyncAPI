﻿using AutoMapper;
using FluentValidation;
using MedSync.Application.Interfaces;
using MedSync.Application.PaginationModel;
using MedSync.Application.Responses;
using MedSync.Application.Validation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static MedSync.Application.Requests.AgendaRequest;

namespace MedSync.Application.Services;

public class AgendaService : BaseService, IAgendaSevice
{
    private Response _response = new();
    private readonly IAgendaRepository _agendaRepository;
    private readonly IMedicoService _medicoService;
    private readonly IHorarioService _horarioService;
    private readonly IValidator<Agenda> _agendaValidator;
    public AgendaService(IAgendaRepository agendaRepository,
        IMedicoService medicoService,
        IHorarioService horarioService,
        IValidator<Agenda> agendaValidator,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AgendaService> logger) : base(mapper, httpContextAccessor, logger)
    {
        _agendaRepository = agendaRepository;
        _medicoService = medicoService;
        _horarioService = horarioService;
        _agendaValidator = agendaValidator;
    }

    public async Task<Response> CreateAsync(AdicionarAgendaRequest agendaRequest)
    {
        var agenda = mapper.Map<Agenda>(agendaRequest);
        agenda.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);
        agenda.ValidacaoCadastrar = true;

        _response = await ExecultarValidacaoResponse(_agendaValidator, agenda);
        if (_response.Error)
            throw new ArgumentException(_response.Status);

        agenda.Medico = mapper.Map<Medico>(await _medicoService.GetIdAsync(agendaRequest.MedicoId)) ??
             throw new KeyNotFoundException("Medico não encontrado em nossa base de dados.");

        if (!await _agendaRepository.CreateAsync(agenda))
            throw new InvalidOperationException("Falha ao criar agenda.");

        foreach (var horario in agendaRequest.Horarios)
        {
            horario.AgendaId = agenda.Id;

            await _horarioService.CreateAsync(horario);
        }

        return ReturnResponseSuccess();
    }

    public async Task<Pagination<AgendaResponse>> GetAllAsync(int page, int pageSize)
    {
        var agendas = mapper.Map<IEnumerable<AgendaResponse>>(await _agendaRepository.GetAllAsync());

        return Paginar(agendas, page, pageSize);
    }

    public async Task<AgendaResponse?> GetIdAsync(Guid id)
    {
        return mapper.Map<AgendaResponse>(await _agendaRepository.GetIdAsync(id));
    }

    public async Task<Pagination<AgendaResponse>> GetMedicoIdAsync(Guid medicoId, int page, int pageSize)
    {
        var agendas = mapper.Map<IEnumerable<AgendaResponse>>(await _agendaRepository.GetMedicoIdAsync(medicoId));

        return Paginar(agendas, page, pageSize);
    }

    public async Task<Response> UpdateAsync(AtualizarAgendaResquet agendaResquest)
    {

        var agenda = mapper.Map<Agenda>(agendaResquest);
        agenda.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), false);
        agenda.ValidacaoCadastrar = false;

        _response = await ExecultarValidacaoResponse(_agendaValidator, agenda);
        if (_response.Error)
            throw new ArgumentException(_response.Status);

        if (!await _agendaRepository.UpdateAsync(agenda))
            throw new InvalidOperationException("Falha ao atualiza agendamento.");

        foreach (var horario in agendaResquest.Horarios)
        {
            horario.AgendaId = agenda.Id;
            await _horarioService.UpdateAsync(horario);
        }

        return ReturnResponseSuccess();
    }

    public async Task<Response> DeleteAsync(Guid id)
    {
        if (!await _agendaRepository.DeleteAsync(id))
            throw new InvalidOperationException("Falha ao deletar agendamento de nossa base de dados.");

        return ReturnResponseSuccess();
    }
}
