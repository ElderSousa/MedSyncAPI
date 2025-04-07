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
    private readonly IAgendaRepository _agendamentoRepository;
    private readonly IPacienteService _pacienteService;
    private readonly IMedicoService _medicoService;
    public AgendaService(IAgendaRepository agendamentoRepository,
        IPacienteService pacienteService,
        IMedicoService medicoService,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AgendaService>logger) : base(mapper, httpContextAccessor, logger)
    {
        _agendamentoRepository = agendamentoRepository;
        _pacienteService = pacienteService;
        _medicoService = medicoService;
    }

    public async Task<Response> CreateAsync(AdicionarAgendaRequest agendamentoRequest)
    {
        try
        {
            var agendamento = mapper.Map<Agenda>(agendamentoRequest);
            agendamento.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);

            _response = ExecultarValidacaoResponse(new AgendaValidation(_agendamentoRepository, true), agendamento);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            agendamento.Paciente = mapper.Map<Paciente>(await _pacienteService.GetIdAsync(agendamentoRequest.PacienteId)) ??
                throw new KeyNotFoundException("Paciente não encontrado em nossa base de dados.");

            agendamento.Medico = mapper.Map<Medico>(await _medicoService.GetIdAsync(agendamentoRequest.MedicoId)) ??
                 throw new KeyNotFoundException("Medico não encontrado em nossa base de dados.");

            if (!await _agendamentoRepository.CreateAsync(agendamento))
                throw new InvalidOperationException("Falha ao criar agendamento.");

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
            return mapper.Map<IEnumerable<AgendaResponse>>(await _agendamentoRepository.GetAllAsync());
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
            return mapper.Map<AgendaResponse>(await _agendamentoRepository.GetIdAsync(id));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "CreateAsync");
            throw;
        }
    }

    public async Task<IEnumerable<AgendaResponse?>> GetPacienteIdAsync(Guid pacienteId)
    {
        try
        {
            return mapper.Map<IEnumerable<AgendaResponse>>(await _agendamentoRepository.GetPacienteIdAsync(pacienteId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetPacienteIdAsync");
            throw;
        }
    }

    public async Task<IEnumerable<AgendaResponse?>> GetMedicoIdAsync(Guid medicoId)
    {
        try
        {
            return mapper.Map<IEnumerable<AgendaResponse>>(await _agendamentoRepository.GetMedicoIdAsync(medicoId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetMedicoIdAsync");
            throw;
        }
    }

    public async Task<Response> UpdateAsync(AtualizarAgendaResquet agendamentoResquest)
    {
        try
        {
            var agendamento = mapper.Map<Agenda>(agendamentoResquest);
            agendamento.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), false);

            _response = ExecultarValidacaoResponse(new AgendaValidation(_agendamentoRepository, false), agendamento);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            if (!await _agendamentoRepository.UpdateAsync(agendamento))
                throw new InvalidOperationException("Falha ao atualiza agendamento.");

        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "CreateAsync");
            throw;
        }

        return ReturnResponseSuccess();
    }

    public async Task<Response> DeleteAsync(Guid id)
    {
        try
        {
            if (!await _agendamentoRepository.DeleteAsync(id))
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
