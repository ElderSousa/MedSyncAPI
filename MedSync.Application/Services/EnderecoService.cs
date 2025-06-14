﻿using AutoMapper;
using FluentValidation;
using MedSync.Application.Interfaces;
using MedSync.Application.Responses;
using MedSync.Application.Validation;
using MedSync.Domain.Entities;
using MedSync.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static MedSync.Application.Requests.EnderecoRequest;

namespace MedSync.Application.Services
{
    public class EnderecoService : BaseService, IEnderecoService
    {
        private Response _response = new();
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IValidator<Endereco> _enderecoValidator;
        public EnderecoService(IEnderecoRepository enderecoRepository,
            IValidator<Endereco> enderecoValidator,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<EnderecoService> logger) : base(mapper, httpContextAccessor, logger)
        {
            _enderecoRepository = enderecoRepository;
            _enderecoValidator = enderecoValidator;
        }

        public async Task<Response> CreateAsync(AdicionarEnderecoRequest enderecoRequest)
        {

            var endereco = mapper.Map<Endereco>(enderecoRequest);
            endereco.AdicionarBaseModel(null, DataHoraAtual(), true);
            endereco.ValidacaoCadastrar = true;

            _response = await ExecultarValidacaoResponse(_enderecoValidator, endereco);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            if (!await _enderecoRepository.CreateAsync(endereco))
                throw new InvalidOperationException("Endereço não adicionado a nossa base de dados.");

            return ReturnResponseSuccess();
        }

        public async Task<EnderecoResponse?> GetIdAsync(Guid id)
        {
            return mapper.Map<EnderecoResponse>(await _enderecoRepository.GetIdAsync(id));
        }

        public async Task<EnderecoResponse?> GetCEPAsync(string cep)
        {

            return mapper.Map<EnderecoResponse>(await _enderecoRepository.GetCEPAsync(cep));
        }

        public async Task<Response> UpdateAsync(AtualizarEnderecoRequest enderecoRequest)
        {
            var endereco = mapper.Map<Endereco>(enderecoRequest);
            endereco.AdicionarBaseModel(null, DataHoraAtual(), false);
            endereco.ValidacaoCadastrar = false;

            _response = await ExecultarValidacaoResponse(_enderecoValidator, endereco);
            if (_response.Error)
                throw new ArgumentException(_response.Status);

            if (!await _enderecoRepository.UpdateAsync(endereco))
                throw new InvalidOperationException("Endereço não atualizado em nossa base de dados.");

            return ReturnResponseSuccess();
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            if (!await _enderecoRepository.DeleteAsync(id))
                throw new ArgumentException("Endereço não excluído da nossa base de dados.");
            return ReturnResponseSuccess();
        }
    }
}
