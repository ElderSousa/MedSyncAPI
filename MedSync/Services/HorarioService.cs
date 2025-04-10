using AutoMapper;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using MedSync.Application.Validation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static MedSync.Application.Requests.HorarioRequest;

namespace MedSync.Application.Services;

public class HorarioService : BaseService, IHorarioService
{
    private Response _response = new();
    private IHorarioRepository _horarioRepository;
    private IAgendaRepository _agendaRepository;
    public HorarioService(IHorarioRepository horarioRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<HorarioService> logger,
        IAgendaRepository agendaRepository) : base(mapper, httpContextAccessor, logger)
    {
        _horarioRepository = horarioRepository;
        _agendaRepository = agendaRepository;
    }

    public async Task<Response> CreateAsync(AdicionarHorarioRequest horarioRequest)
    {
        try
        {
            var horario = mapper.Map<Horario>(horarioRequest);
            horario.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);

            _response = ExecultarValidacaoResponse(new HorarioValidation(_horarioRepository, _agendaRepository, true), horario);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            if (!await _horarioRepository.CreateAsync(horario))
                throw new InvalidOperationException("Falha ao criar horário.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "CreateAsync");
            throw;
        }

        return ReturnResponseSuccess();
    }

    public async Task<IEnumerable<HorarioResponse?>> GetAllAsync()
    {
        try
        {
            return mapper.Map<IEnumerable<HorarioResponse>>(await _horarioRepository.GetAllAsync());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetAllAsync");
            throw;
        }
    }
    public async Task<HorarioResponse?> GetIdAsync(Guid id)
    {
        try
        {
            return mapper.Map<HorarioResponse>(await _horarioRepository.GetIdAsync(id));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetIdAsync");
            throw;
        }
    }

    public async Task<IEnumerable<HorarioResponse?>> GetAgendaIdAsync(Guid agendaId)
    {
        try
        {
            return mapper.Map<IEnumerable<HorarioResponse>>(await _horarioRepository.GetAgendaIdAsync(agendaId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetAgendaIdAsync");
            throw;
        }
    }

    public async Task<Response> UpdateAsync(AtualizarHorarioRequest horarioResquest)
    {
        try
        {
            var horario = mapper.Map<Horario>(horarioResquest);
            horario.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), false);

            _response = ExecultarValidacaoResponse(new HorarioValidation(_horarioRepository, _agendaRepository, false), horario);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            if (!await _horarioRepository.UpdateAsync(horario))
                throw new InvalidOperationException("Falha ao atualizar horário.");
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
            if (!await _horarioRepository.DeleteAsync(id))
                throw new InvalidOperationException("Falha ao excluir horário.");

            return ReturnResponseSuccess();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "DeleteAsync");
            throw;
        }
    }

    public async Task<IEnumerable<HorarioResponse?>> GetAgendadoFalseAsync()
    {
        try
        {
            return mapper.Map<IEnumerable<HorarioResponse>>(await _horarioRepository.GetAgendadoFalseAsync());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "GetAgendadoFalseAsync");
            throw;
        }
    }

    public async Task<Response> UpdateStatusAsync(Guid id, bool agendado)
    {
        try
        {
            var horario = GetIdAsync(id);
            if (horario == null)
                throw new KeyNotFoundException("Horário não encontrado em nossa base de dados.");

            if (!await _horarioRepository.UpdateStatusAsync(id, agendado))
                throw new InvalidOperationException("Falha ao atualizar status.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "UpdateAsync");
            throw;
        }

        return ReturnResponseSuccess();
    }
}
