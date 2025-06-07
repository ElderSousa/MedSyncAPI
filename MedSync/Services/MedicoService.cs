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
using static MedSync.Application.Requests.MedicoResquest;

namespace MedSync.Application.Services;

public class MedicoService : BaseService, IMedicoService
{
    private Response _response = new();
    private readonly IMedicoRepository _medicoRepository;
    private readonly IPessoaService _pessoaService;
    private readonly ITelefoneService _telefoneService;
    private readonly IValidator<Medico> _medicoValidation;
    public MedicoService(IMedicoRepository medicoRepository,
        IPessoaService pessoaService,
        ITelefoneService telefoneService,
        IValidator<Medico> medicoValidator,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<MedicoService> logger) : base(mapper, httpContextAccessor, logger)
    {
        _medicoRepository = medicoRepository;
        _pessoaService = pessoaService;
        _telefoneService = telefoneService;
        _medicoValidation = medicoValidator;
    }

    public async Task<Response> CreateAsync(AdicionarMedicoRequest medicoRequest)
    {
        var medico = mapper.Map<Medico>(medicoRequest);
        medico.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);
        medico.ValidacaoCadastrar = true;

        _response = await ExecultarValidacaoResponse(_medicoValidation, medico);
        if (_response.Error)
            throw new ArgumentException(_response.Status);

        var pessoa = await _pessoaService.GetCPFAsync(medicoRequest.Pessoa.CPF!);
        if (pessoa == null || pessoa.Id == Guid.Empty)
            _response = await _pessoaService.CreateAsync(medicoRequest.Pessoa);

        medico.Pessoa = mapper.Map<Pessoa>(await _pessoaService.GetCPFAsync(medicoRequest.Pessoa.CPF!));
        medico.PessoaId = medico.Pessoa.Id;

        if (!await _medicoRepository.CreateAsync(medico))
            throw new InvalidOperationException("Falha ao adicionar médico em nossa base de dados.");

        for (var i = 0; i < medicoRequest.Telefones.Count(); i++)
        {
            medicoRequest.Telefones[i].MedicoId = medico.Id;
            await _telefoneService.CreateAsync(medicoRequest.Telefones[i]);
            medico.Telefones.Add(mapper.Map<Telefone>(medicoRequest.Telefones[i]));
        }

        return ReturnResponseSuccess();
    }
    public async Task<Pagination<MedicoResponse>> GetAllAsync(int page, int pageSize)
    {
        var medicos = mapper.Map<IEnumerable<MedicoResponse>>(await _medicoRepository.GetAllAsync());

        return Paginar(medicos, page, pageSize);
    }

    public async Task<MedicoResponse?> GetIdAsync(Guid id)
    {
        return mapper.Map<MedicoResponse>(await _medicoRepository.GetIdAsync(id));
    }

    public async Task<MedicoResponse?> GetCRMAsync(string crm)
    {
        return mapper.Map<MedicoResponse>(await _medicoRepository.GetCRMAsync(crm));
    }

    public async Task<Response> UpdateAsync(AtualizarMedicoRequest medicoRequest)
    {
        var medico = mapper.Map<Medico>(medicoRequest);
        medico.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), false);
        medico.ValidacaoCadastrar = false;

        _response = await ExecultarValidacaoResponse(_medicoValidation, medico);
        if (_response.Error)
            throw new ArgumentException(_response.Status);

        if (!await _medicoRepository.UpdateAsync(medico))
            throw new InvalidOperationException("Falha na atualização de médico em nossa base de dados.");

        await _pessoaService.UpdateAsync(medicoRequest.Pessoa);

        foreach (var telefone in medicoRequest.Telefones)
            await _telefoneService.UpdateAsync(telefone);

        return ReturnResponseSuccess();
    }

    public async Task<Response> DeleteAsync(Guid id)
    {
        var deletado = await _medicoRepository.DeleteAsync(id);
        if (!deletado)
            throw new InvalidOperationException("Falha na exclusão da nossa base de dados.");

        return ReturnResponseSuccess();
    }
}
