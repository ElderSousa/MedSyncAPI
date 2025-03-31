using AutoMapper;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using MedSync.Application.Validation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static MedSync.Application.Requests.MedicoResquest;

namespace MedSync.Application.Services
{
    public class MedicoService : BaseService, IMedicoService
    {
        private Response _response = new();
        private readonly IMedicoRepository _medicoRepository;
        private readonly IPessoaService _pessoaService;
        private readonly ITelefoneService _telefoneService;
        public MedicoService(IMedicoRepository medicoRepository,
            IPessoaService pessoaService,
            ITelefoneService telefoneService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<MedicoService> logger) : base(mapper, httpContextAccessor, logger)
        {
            _medicoRepository = medicoRepository;
            _pessoaService = pessoaService;
            _telefoneService = telefoneService;
        }

        public async Task<Response> CreateAsync(AdicionarMedicoRequest medicoRequest)
        {
            try
            {
                var medico = mapper.Map<Medico>(medicoRequest);
                medico.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), true);

                _response = ExecultarValidacaoResponse(new MedicoValidation(_medicoRepository, true), medico);
                if (_response.Error)
                    throw new ArgumentException(_response.Status);

                var pessoa = await _pessoaService.GetCPFAsync(medicoRequest.Pessoa.CPF!);
                if (pessoa == null)
                    _response = await _pessoaService.CreateAsync(medicoRequest.Pessoa);
 
                medico.Pessoa = mapper.Map<Pessoa>(await _pessoaService.GetCPFAsync(medicoRequest.Pessoa.CPF!));
                medico.PessoaId = medico.Pessoa.Id;

                if (!await _medicoRepository.CreateAsync(medico))
                    throw new InvalidOperationException("Falha ao adicionar médico em nossa base de dados.");

                for (var i = 0; i < medicoRequest.Telefones.Count(); i++)
                {
                    medicoRequest.Telefones[i].MedicoId = medico.Id;
                    await _telefoneService.CreateAsync(medicoRequest.Telefones[i]);
                    medico.Telefones[i] = mapper.Map<Telefone>(medicoRequest.Telefones[i]);      
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, "CreateAsync");
                throw;
            }
            return ReturnResponseSuccess();
        }
        public async Task<IEnumerable<MedicoResponse?>> GetAllAsync()
        {
            try
            {
                return mapper.Map<IEnumerable<MedicoResponse>>(await _medicoRepository.GetAllAsync());
            }
            catch (Exception ex)
            {

                logger.LogError(ex, ex.Message, "GetAllAsync");
                throw;
            }
        }

        public async Task<MedicoResponse?> GetIdAsync(Guid id)
        {
            try
            {
                return mapper.Map<MedicoResponse>(await _medicoRepository.GetIdAsync(id));
            }
            catch (Exception ex)
            {

                logger.LogError(ex, ex.Message, "GetIdAsync");
                throw;
            }
        }

        public async Task<MedicoResponse?> GetCRMAsync(string crm)
        {
            try
            {
                return mapper.Map<MedicoResponse>(await _medicoRepository.GetCRMAsync(crm));
            }
            catch (Exception ex)
            {

                logger.LogError(ex, ex.Message, "GetCRMAsync");
                throw;
            }
        }

        public async Task<Response> UpdateAsync(AtualizarMedicoRequest medicoRequest)
        {
            try
            {
                var medico = mapper.Map<Medico>(medicoRequest);
                medico.AdicionarBaseModel(ObterUsuarioLogadoId(), DataHoraAtual(), false);

                _response = ExecultarValidacaoResponse(new MedicoValidation(_medicoRepository, false), medico);
                if (_response.Error)
                    throw new ArgumentException(_response.Status);

                if (!await _medicoRepository.UpdateAsync(medico))
                    throw new InvalidOperationException("Falha na atualização de médico em nossa base de dados.");

                await _pessoaService.UpdateAsync(medicoRequest.Pessoa);

                foreach (var telefone in medicoRequest.Telefones)
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
                var deletado = await _medicoRepository.DeleteAsync(id);
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
}
