using AutoMapper;
using FluentValidation;
using MedSync.Application.Interfaces;
using MedSync.Application.PaginationModel;
using MedSync.Application.Responses;
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
    private IValidator<Horario> _horarioValidator;
    public HorarioService(IHorarioRepository horarioRepository,
        IValidator<Horario> horarioValidator,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<HorarioService> logger) : base(mapper, httpContextAccessor, logger)
    {
        _horarioRepository = horarioRepository;
        _horarioValidator = horarioValidator;
    }

    public async Task<Response> CreateAsync(AdicionarHorarioRequest horarioRequest)
    {
        var horario = mapper.Map<Horario>(horarioRequest);
        horario.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);
        horario.ValidacaoCadastrar = true;

        _response = await ExecultarValidacaoResponse(_horarioValidator, horario);
        if (_response.Error)
            throw new ArgumentException(_response.Status);

        if (!await _horarioRepository.CreateAsync(horario))
            throw new InvalidOperationException("Falha ao criar horário.");

        return ReturnResponseSuccess();
    }

    public async Task<Pagination<HorarioResponse>> GetAllAsync(int page, int pageSize)
    {
        var horarios = mapper.Map<IEnumerable<HorarioResponse>>(await _horarioRepository.GetAllAsync());

        return Paginar(horarios, page, pageSize);
    }
    public async Task<HorarioResponse?> GetIdAsync(Guid id)
    {
        return mapper.Map<HorarioResponse>(await _horarioRepository.GetIdAsync(id));
    }

    public async Task<Pagination<HorarioResponse>> GetAgendaIdAsync(Guid agendaId, int page, int pageSize)
    {
        var horarios = mapper.Map<IEnumerable<HorarioResponse>>(await _horarioRepository.GetAgendaIdAsync(agendaId));

        return Paginar(horarios, page, pageSize);
    }

    public async Task<Response> UpdateAsync(AtualizarHorarioRequest horarioResquest)
    {
        var horario = mapper.Map<Horario>(horarioResquest);
        horario.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), false);
        horario.ValidacaoCadastrar = false;

        _response = await ExecultarValidacaoResponse(_horarioValidator, horario);
        if (_response.Error)
            throw new ArgumentException(_response.Status);

        if (!await _horarioRepository.UpdateAsync(horario))
            throw new InvalidOperationException("Falha ao atualizar horário.");

        return ReturnResponseSuccess();
    }

    public async Task<Response> DeleteAsync(Guid id)
    {
        if (!await _horarioRepository.DeleteAsync(id))
            throw new InvalidOperationException("Falha ao excluir horário.");

        return ReturnResponseSuccess();
    }

    public async Task<Pagination<HorarioResponse>> GetAgendadoFalseAsync(int page, int pageSize)
    {
        var horarios = mapper.Map<IEnumerable<HorarioResponse>>(await _horarioRepository.GetAgendadoFalseAsync());

        return Paginar(horarios, page, pageSize);
    }

    public async Task<Response> UpdateStatusAsync(Guid id, bool agendado)
    {
        var horario = await GetIdAsync(id);
        if (horario == null)
            throw new KeyNotFoundException("Horário não encontrado em nossa base de dados.");

        if (!await _horarioRepository.UpdateStatusAsync(id, agendado))
            throw new InvalidOperationException("Falha ao atualizar status.");

        return ReturnResponseSuccess();
    }
}
