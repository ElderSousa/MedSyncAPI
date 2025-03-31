using AutoMapper;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using MedSync.Application.Validation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static MedSync.Application.Requests.PacienteRequest;

namespace MedSync.Application.Services;

public class PacienteService : BaseService, IPacienteService
{
    private Response _response = new();
    private readonly IPacienteRepository _pacienteRepository;
    private readonly IPessoaService _pessoaService;
    private readonly IEnderecoService _enderecoService;
    private readonly ITelefoneService _telefoneService;
    public PacienteService(IPacienteRepository pacienteRepository,
        IPessoaService pessoaService,
        IEnderecoService enderecoService,
        ITelefoneService telefoneService,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<EnderecoService> logger) : base(mapper, httpContextAccessor, logger)
    {
        _pacienteRepository = pacienteRepository;
        _pessoaService = pessoaService;
        _enderecoService = enderecoService;
        _telefoneService = telefoneService;
    }

    public async Task<Response> CreateAsync(AdicionarPacienteRequest pacienteRequest)
    {
        try
        {
            var paciente = mapper.Map<Paciente>(pacienteRequest);
            paciente.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);

            _response = ExecultarValidacaoResponse(new PacienteValidation(_pacienteRepository, true), paciente);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            var pessoa = await _pessoaService.GetCPFAsync(pacienteRequest.Pessoa.CPF!);
            if (pessoa == null)
                await _pessoaService.CreateAsync(pacienteRequest.Pessoa);
            paciente.PessoaId = pessoa!.Id;

            if (!await _pacienteRepository.CreateAsync(paciente))
                throw new InvalidOperationException("Falha ao adicionar paciente em nossa base de dados.");

            pacienteRequest.Endereco.PacienteId = paciente.Id;
            await _enderecoService.CreateAsync(pacienteRequest.Endereco);

            foreach(var telefone in pacienteRequest.Telefones)
            {
                telefone.PacienteId = paciente.Id;
                await _telefoneService.CreateAsync(telefone);
                paciente.Telefones.Add(mapper.Map<Telefone>(telefone));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, "CreateAsync");
            throw;
        }
        return ReturnResponseSuccess();
    }
    public async Task<IEnumerable<PacienteResponse?>> GetAllAsync()
    {
        try
        {
            return mapper.Map<IEnumerable<PacienteResponse>>(await _pacienteRepository.GetAllAsync());
        }
        catch (Exception ex)
        {

            logger.LogError(ex, ex.Message, "GetAllAsync");
            throw;
        }
    }

    public async Task<PacienteResponse?> GetIdAsync(Guid id)
    {
        try
        {
            return mapper.Map<PacienteResponse>(await _pacienteRepository.GetIdAsync(id));
        }
        catch (Exception ex)
        {

            logger.LogError(ex, ex.Message, "GetIdAsync");
            throw;
        }
    }

    public async Task<Response> UpdateAsync(AtualizarPacienteRequest pacienteRequest)
    {
        try
        {
            var paciente = mapper.Map<Paciente>(pacienteRequest);
            paciente.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), false);

            _response = ExecultarValidacaoResponse(new PacienteValidation(_pacienteRepository, false), paciente);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            await _pessoaService.UpdateAsync(pacienteRequest.Pessoa);

            await _enderecoService.UpdateAsync(pacienteRequest.Endereco);

            foreach(var telefone in pacienteRequest.Telefones)
                await _telefoneService.UpdateAsync(telefone);
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
            var deletado = await _pacienteRepository.DeleteAsync(id);
            if (!deletado)
                throw new InvalidOperationException("Falha na exclusão da nossa base de dados.");

            return ReturnResponseSuccess();
        }
        catch (Exception ex)
        {

            logger.LogError(ex, ex.Message, "DeleteAsync");
            throw;
        }
    }
}

