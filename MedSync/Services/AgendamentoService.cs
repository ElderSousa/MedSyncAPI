using System.Runtime.InteropServices;
using AutoMapper;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using MedSync.Application.Validation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static MedSync.Application.Requests.AgendamentoRequest;

namespace MedSync.Application.Services;

public class AgendamentoService : BaseService, IAgendamentoService
{
    private Response _response = new();
    private readonly IAgendamentoRepository _agendamentoRepository;
    private readonly IAgendaRepository _agendaRepository;
    private readonly IMedicoRepository _medicoRepository;
    private readonly IPacienteRepository _pacienteRepository;
    private readonly IHorarioRepository _horarioRepository;
    private readonly IHorarioService _horarioService;

    public AgendamentoService(IAgendamentoRepository agendamentoRepository,
        IAgendaRepository agendaRepository,
        IMedicoRepository medicoRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AgendamentoService> logger,
        IPacienteRepository pacienteRepository,
        IHorarioRepository horarioRepository,
        IHorarioService horarioService) : base(mapper, httpContextAccessor, logger)
    {
        _agendamentoRepository = agendamentoRepository;
        _agendaRepository = agendaRepository;
        _medicoRepository = medicoRepository;
        _pacienteRepository = pacienteRepository;
        _horarioRepository = horarioRepository;
        _horarioService = horarioService;
    }

    public async Task<Response> CreateAsync(AdicionaAgendamentoRequest agendamentoResquest)
    {
        try
        {
            var agendamento = mapper.Map<Agendamento>(agendamentoResquest);
            agendamento.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);

            _response = ExecultarValidacaoResponse(new AgendamentoValidation(_agendamentoRepository, _agendaRepository, _medicoRepository, _pacienteRepository, _horarioRepository, true), agendamento);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            if (!await _agendamentoRepository.CreateAsync(agendamento))
                throw new InvalidOperationException("Falha ao criar agenda.");

            var horarios = await _horarioService.GetAgendaIdAsync(agendamento.AgendaId);
            if (!horarios.Any())
                throw new KeyNotFoundException("Horários não encontrado em nossa base de dados!");

            var horario = horarios.ToList().Single(h => h!.Hora == agendamento.Horario);
            horario!.Agendado = true;

            await _horarioService.UpdateStatusAsync(horario.Id, horario.Agendado);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "CreateAsync");
            throw;
        }
        return ReturnResponseSuccess();
    }

    public async Task<IEnumerable<AgendamentoResponse?>> GetAllAsync()
    {
        try
        {
            return mapper.Map<IEnumerable<AgendamentoResponse>>(await _agendamentoRepository.GetAllAsync());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetAllAsync");
            throw;
        }
    }

    public async Task<AgendamentoResponse?> GetIdAsync(Guid id)
    {
        try
        {
            return mapper.Map<AgendamentoResponse>(await _agendamentoRepository.GetIdAsync(id));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetIdAsync");
            throw;
        }
    }

    public async Task<IEnumerable<AgendamentoResponse?>> GetMedicoIdAsync(Guid medicoId)
    {
        try
        {
            return mapper.Map<IEnumerable<AgendamentoResponse>>(await _agendamentoRepository.GetMedicoIdAsync(medicoId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetMedicoIdAsync");
            throw;
        }
    } 
    
    public async Task<IEnumerable<AgendamentoResponse?>> GetPacienteIdAsync(Guid pacienteId)
    {
        try
        {
            return mapper.Map<IEnumerable<AgendamentoResponse>>(await _agendamentoRepository.GetPacienteIdAsync(pacienteId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetPacienteIdAsync");
            throw;
        }
    }

    public async Task<IEnumerable<AgendamentoResponse?>> GetAgendaIdAsync(Guid agendaId)
    {
        try
        {
            return mapper.Map<IEnumerable<AgendamentoResponse>>(await _agendamentoRepository.GetAgendaIdAsync(agendaId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetAgendaIdAsync");
            throw;
        }
    }

    public async Task<Response> UpdateAsync(AtualizarAgendamentoRequest agendamentoRequest)
    {
        try
        {
            var agendamento = mapper.Map<Agendamento>(agendamentoRequest);
            agendamento.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), false);

            _response = ExecultarValidacaoResponse(new AgendamentoValidation(_agendamentoRepository, _agendaRepository, _medicoRepository, _pacienteRepository, _horarioRepository, false), agendamento);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            if (!await _agendamentoRepository.UpdateAsync(agendamento))
                throw new InvalidOperationException("Falha ao atualizar agendamento.");
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
            if (!await _agendamentoRepository.DeleteAsync(id))
                throw new InvalidOperationException("Falha ao excluir o agendamento.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "DeleteAsync");
            throw;
        }

        return ReturnResponseSuccess();
    }
}
