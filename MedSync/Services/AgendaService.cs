using AutoMapper;
using MedSync.Application.Interfaces;
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
    private readonly IMedicoRepository _medicoRepository;
    private readonly IHorarioService _horarioService;
    public AgendaService(IAgendaRepository agendaRepository,
        IMedicoService medicoService,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AgendaService> logger,
        IMedicoRepository medicoRepository,
        IHorarioService horarioService) : base(mapper, httpContextAccessor, logger)
    {
        _agendaRepository = agendaRepository;
        _medicoService = medicoService;
        _medicoRepository = medicoRepository;
        _horarioService = horarioService;
    }

    public async Task<Response> CreateAsync(AdicionarAgendaRequest agendaRequest)
    {
        try
        {
            var agenda = mapper.Map<Agenda>(agendaRequest);
            agenda.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);

            _response = ExecultarValidacaoResponse(new AgendaValidation(_agendaRepository, _medicoRepository, true), agenda);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            agenda.Medico = mapper.Map<Medico>(await _medicoService.GetIdAsync(agendaRequest.MedicoId)) ??
                 throw new KeyNotFoundException("Medico não encontrado em nossa base de dados.");

            if (!await _agendaRepository.CreateAsync(agenda))
                throw new InvalidOperationException("Falha ao criar agendamento.");

            foreach (var horario in agendaRequest.Horarios)
            {
                horario.AgendaId = agenda.Id;
                await _horarioService.CreateAsync(horario);
            }

        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "CreateAsync");
            throw;
        }

        return ReturnResponseSuccess();
    }

    public async Task<IEnumerable<AgendaResponse?>> GetAllAsync()
    {
        try
        {
            return mapper.Map<IEnumerable<AgendaResponse>>(await _agendaRepository.GetAllAsync());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetAllAsync");
            throw;
        }
    }

    public async Task<AgendaResponse?> GetIdAsync(Guid id)
    {
        try
        {
            return mapper.Map<AgendaResponse>(await _agendaRepository.GetIdAsync(id));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetIdAsync");
            throw;
        }
    }

    public async Task<IEnumerable<AgendaResponse?>> GetMedicoIdAsync(Guid medicoId)
    {
        try
        {
            return mapper.Map<IEnumerable<AgendaResponse>>(await _agendaRepository.GetMedicoIdAsync(medicoId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetMedicoIdAsync");
            throw;
        }
    }

    public async Task<Response> UpdateAsync(AtualizarAgendaResquet agendaResquest)
    {
        try
        {
            var agenda = mapper.Map<Agenda>(agendaResquest);
            agenda.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), false);

            _response = ExecultarValidacaoResponse(new AgendaValidation(_agendaRepository, _medicoRepository, false), agenda);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            if (!await _agendaRepository.UpdateAsync(agenda))
                throw new InvalidOperationException("Falha ao atualiza agendamento.");

            foreach (var horario in agendaResquest.Horarios)
            {
                horario.AgendaId = agenda.Id;
                await _horarioService.UpdateAsync(horario);
            }

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
            if (!await _agendaRepository.DeleteAsync(id))
                throw new InvalidOperationException("Falha ao deletar agendamento de nossa base de dados.");

            return ReturnResponseSuccess();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "DeleteAsync");
            throw;
        }
    }
}
