using AutoMapper;
using FluentValidation;
using MedSync.Application.Interfaces;
using MedSync.Application.PaginationModel;
using MedSync.Application.Responses;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static MedSync.Application.Requests.TelefoneRequest;

namespace MedSync.Application.Services;

public class TelefoneService : BaseService, ITelefoneService
{
    private Response _response = new();
    private readonly ITelefoneRepository _telefoneRepository;
    private readonly IValidator<Telefone> _telefoneValidation;
    public TelefoneService(ITelefoneRepository telefoneRepository,
        IValidator<Telefone> telefoneValidation,
        IMapper mapper, IHttpContextAccessor httpContextAccessor,
        ILogger<TelefoneService> logger) : base(mapper, httpContextAccessor, logger)
    {
        _telefoneRepository = telefoneRepository;
        _telefoneValidation = telefoneValidation;
    }

    public async Task<Response> CreateAsync(AdicionarTelefoneRequest telefoneRequest)
    {
        var telefone = mapper.Map<Telefone>(telefoneRequest);
        telefone.AdicionarBaseModel(null, DataHoraAtual(), true);
        telefone.ValidacaoCadastrar = true;

        _response = await ExecultarValidacaoResponse(_telefoneValidation, telefone);
        if (_response.Error)
            throw new ArgumentException(_response.Status);

        if (!await _telefoneRepository.CreateAsync(telefone))
            throw new InvalidOperationException("Telefone não adicionado em nossa base de dados.");

        return ReturnResponseSuccess();
    }

    public async Task<TelefoneResponse?> GetIdAsync(Guid id)
    {
        return mapper.Map<TelefoneResponse>(await _telefoneRepository.GetIdAsync(id));
    }

    public async Task<Pagination<TelefoneResponse>> GetAllAsync(int page, int pageSize)
    {
        var telefones = mapper.Map<IEnumerable<TelefoneResponse>>(await _telefoneRepository.GetAllAsync());
        return Paginar(telefones, page, pageSize);
    }

    public async Task<Pagination<TelefoneResponse>> GetMedicoIdAsync(Guid medicoId, int page, int pageSize)
    {
        var telefones = mapper.Map<IEnumerable<TelefoneResponse>>(await _telefoneRepository.GetMedicoIdAsync(medicoId));
        return Paginar(telefones, page, pageSize);
    }

    public async Task<Pagination<TelefoneResponse>> GetPacienteIdAsync(Guid pacienteId, int page, int pageSize)
    {
        var telefones = mapper.Map<IEnumerable<TelefoneResponse>>(await _telefoneRepository.GetPacienteIdAsync(pacienteId));
        return Paginar(telefones, page, pageSize);
    }

    public async Task<TelefoneResponse?> GetNumeroAsync(string numero)
    {
        return mapper.Map<TelefoneResponse>(await _telefoneRepository.GetNumeroAsync(numero));
    }

    public async Task<Response> UpdateAsync(AtualizarTelefoneRequest telefoneRequest)
    {
        var telefone = mapper.Map<Telefone>(telefoneRequest);
        telefone.AdicionarBaseModel(null, DataHoraAtual(), false);
        telefone.ValidacaoCadastrar = false;

        _response = await ExecultarValidacaoResponse(_telefoneValidation, telefone);
        if (_response.Error)
            throw new ArgumentException(_response.Status);

        if (!await _telefoneRepository.UpdateAsync(telefone))
            throw new InvalidOperationException("Telefone não atualizado em nossa base de dados.");

        return ReturnResponseSuccess();
    }
    public async Task<Response> DeleteAsync(Guid id)
    {
        if (!await _telefoneRepository.DeleteAsync(id))
            throw new InvalidOperationException("Telefone não excluído da nossa base de dados.");

        return ReturnResponseSuccess();
    }

}
