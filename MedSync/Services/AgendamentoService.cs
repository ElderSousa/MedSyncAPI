using System.Linq.Expressions;
using System.Runtime.InteropServices;
using AutoMapper;
using FluentValidation;
using MedSync.Application.Interfaces;
using MedSync.Application.PaginationModel;
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
    private readonly IHorarioService _horarioService;
    private readonly IValidator<Agendamento> _agendamentoValidator;

    public AgendamentoService(IAgendamentoRepository agendamentoRepository,
        IHorarioService horarioService,
        IValidator<Agendamento> agendamentoValidator,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AgendamentoService> logger) : base(mapper, httpContextAccessor, logger)
    {
        _agendamentoRepository = agendamentoRepository;
        _horarioService = horarioService;
        _agendamentoValidator = agendamentoValidator;
    }

    public async Task<Response> CreateAsync(AdicionaAgendamentoRequest agendamentoResquest)
    {
        try
        {
            var agendamento = mapper.Map<Agendamento>(agendamentoResquest);
            agendamento.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);
            agendamento.ValidacaoCadastrar = true;

            _response = await ExecultarValidacaoResponse(_agendamentoValidator, agendamento);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            if (!await _agendamentoRepository.CreateAsync(agendamento))
                throw new InvalidOperationException("Falha ao criar agenda.");

            var horarios = await _horarioService.GetAgendaIdAsync(agendamento.AgendaId, int.MaxValue, int.MaxValue);
            if (!horarios.Itens.Any())
                throw new KeyNotFoundException("Horários não encontrado em nossa base de dados!");

            var horario = horarios.Itens.ToList().Single(h => h!.Hora == agendamento.Horario);
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

    public async Task<Pagination<AgendamentoResponse>> GetAllAsync(int page, int pageSize)
    {
        try
        {
            var agendamentos = mapper.Map<IEnumerable<AgendamentoResponse>>(await _agendamentoRepository.GetAllAsync());

            return Paginar(agendamentos, page, pageSize);
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

    public async Task<Pagination<AgendamentoResponse>> GetMedicoIdAsync(Guid medicoId, int page, int pageSize)
    {
        try
        {
            var agendamentos = mapper.Map<IEnumerable<AgendamentoResponse>>(await _agendamentoRepository.GetMedicoIdAsync(medicoId));

            return Paginar(agendamentos, page, pageSize);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetMedicoIdAsync");
            throw;
        }
    } 
    
    public async Task<Pagination<AgendamentoResponse>> GetPacienteIdAsync(Guid pacienteId, int page, int pageSize)
    {
        try
        {
            var agendamentos =  mapper.Map<IEnumerable<AgendamentoResponse>>(await _agendamentoRepository.GetPacienteIdAsync(pacienteId));

            return Paginar(agendamentos, page, pageSize);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetPacienteIdAsync");
            throw;
        }
    }

    public async Task<Pagination<AgendamentoResponse>> GetAgendaIdAsync(Guid agendaId, int page, int pageSize)
    {
        try
        {
            var agendamentos = mapper.Map<IEnumerable<AgendamentoResponse>>(await _agendamentoRepository.GetAgendaIdAsync(agendaId));

            return Paginar(agendamentos, page, pageSize);
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
            agendamento.ValidacaoCadastrar = false;

            _response = await ExecultarValidacaoResponse(_agendamentoValidator, agendamento);
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
