using AutoMapper;
using MedSync.Application.Interfaces;
using MedSync.Application.Requests;
using MedSync.Application.Responses;
using MedSync.Application.Validation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static MedSync.Application.Requests.AgendamentoRequest;

namespace MedSync.Application.Services;

public class AgendamentoService : BaseService, IAgendamentoSevice
{
    private Response _response = new();
    private readonly IAgendamentoRepository _agendamentoRepository;
    private readonly IPacienteService _pacienteService;
    private readonly IMedicoService _medicoService;
    public AgendamentoService(IAgendamentoRepository agendamentoRepository,
        IPacienteService pacienteService,
        IMedicoService medicoService,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AgendamentoService>logger) : base(mapper, httpContextAccessor, logger)
    {
        _agendamentoRepository = agendamentoRepository;
        _pacienteService = pacienteService;
        _medicoService = medicoService;
    }

    public async Task<Response> CreateAsync(AdicionarAgendamentoRequest agendamentoRequest)
    {
        try
        {
            var agendamento = mapper.Map<Agendamento>(agendamentoRequest);
            agendamento.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);

            _response = ExecultarValidacaoResponse(new AgendamentoValidation(_agendamentoRepository, true), agendamento);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            agendamento.Paciente = mapper.Map<Paciente>(await _pacienteService.GetIdAsync(agendamentoRequest.PacienteId));
            agendamento.Medico = mapper.Map<Medico>(await _medicoService.GetIdAsync(agendamentoRequest.MedicoId));

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
            logger.LogError(ex, ex.Message, "CreateAsync");
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

    public async Task<Response> UpdateAsync(AtualizarAgendamentoResquet agendamentoResquest)
    {
        try
        {
            var agendamento = mapper.Map<Agendamento>(agendamentoResquest);
            agendamento.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), false);

            _response = ExecultarValidacaoResponse(new AgendamentoValidation(_agendamentoRepository, false), agendamento);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            await _pacienteService.UpdateAsync(agendamentoResquest.Paciente);
            await _medicoService.UpdateAsync(agendamentoResquest.Medico);

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
