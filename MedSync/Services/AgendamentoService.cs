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
    private IAgendamentoRepository _agendamentoRepository;
    private IAgendaRepository _agendaRepository;
    private IMedicoRepository _medicoRepository;

    public AgendamentoService(IAgendamentoRepository agendamentoRepository,
        IAgendaRepository agendaRepository,
        IMedicoRepository medicoRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AgendamentoService> logger) : base(mapper, httpContextAccessor, logger)
    {
        _agendamentoRepository = agendamentoRepository;
        _agendaRepository = agendaRepository;
        _medicoRepository = medicoRepository;
    }

    public async Task<Response> CreateAsync(AdicionaAgendamentoRequest agendamentoResquest)
    {
        try
        {
            var agendamento = mapper.Map<Agendamento>(agendamentoResquest);
            agendamento.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);

            _response = ExecultarValidacaoResponse(new AgendamentoValidation(_agendamentoRepository, _agendaRepository, _medicoRepository, true), agendamento);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            if (!await _agendamentoRepository.UpdateAsync(agendamento))
                throw new InvalidOperationException("Falha ao criar agenda.");

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

    public async Task<IEnumerable<AgendamentoResponse?>> GetAgendaIdAsync(Guid agendaId)
    {
        try
        {
            return mapper.Map<IEnumerable<AgendamentoResponse>>(await _agendamentoRepository.GetAgendaIdAsync(agendaId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "");
            throw;
        }
    }

    public async Task<Response> UpdateAsync(AtualizarAgendamentoRequest agendamentoRequest)
    {
        try
        {
            var agendamento = mapper.Map<Agendamento>(agendamentoRequest);
            agendamento.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), false);

            _response = ExecultarValidacaoResponse(new AgendamentoValidation(_agendamentoRepository, _agendaRepository, _medicoRepository, false), agendamento);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            if (!await _agendamentoRepository.UpdateAsync(agendamento))
                throw new InvalidOperationException("Falha ao atualizar agendamento.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "");
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
            logger.LogError(ex, ex.Message, "");
            throw;
        }

        return ReturnResponseSuccess();
    }
}
