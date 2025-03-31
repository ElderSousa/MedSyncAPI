﻿using AutoMapper;
using MedSync.Application.Responses;
using MedSync.Domain.Entities;
using static MedSync.Application.Requests.AgendamentoRequest;
using static MedSync.Application.Requests.EnderecoRequest;
using static MedSync.Application.Requests.MedicoResquest;
using static MedSync.Application.Requests.PacienteRequest;
using static MedSync.Application.Requests.PessoaRequest;
using static MedSync.Application.Requests.TelefoneRequest;

namespace MedSync.Application.Mappings;

public class DomainToDTOMappingProfile : Profile
{
    public DomainToDTOMappingProfile()
    {
        CreateMap<Pessoa, AdicionarPessoaRequest>().ReverseMap();
        CreateMap<Pessoa, PessoaResponse>().ReverseMap();
        CreateMap<AtualizarPessoaRequest, Pessoa>().ReverseMap();
        CreateMap<Endereco, EnderecoResponse>().ReverseMap();
        CreateMap<Endereco, AdicionarEnderecoRequest>().ReverseMap();
        CreateMap<Endereco, AtualizarEnderecoRequest>().ReverseMap();
        CreateMap<Telefone, TelefoneResponse>().ReverseMap();
        CreateMap<Telefone, AdicionarTelefoneRequest>().ReverseMap();
        CreateMap<Telefone, AtualizarTelefoneRequest>().ReverseMap();
        CreateMap<Medico, MedicoResponse>()
            .ForMember(dest => dest.Pessoa, opt => opt.MapFrom(src => src.Pessoa))
            .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones)).ReverseMap();
        CreateMap<Medico, AdicionarMedicoRequest>().ReverseMap();
        CreateMap<Medico, AtualizarMedicoRequest>().ReverseMap();
        CreateMap<Paciente, PacienteResponse>()
            .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones)).ReverseMap();
        CreateMap<Paciente, AtualizarPacienteRequest>().ReverseMap();
        CreateMap<Paciente, AdicionarPacienteRequest>().ReverseMap();
        CreateMap<Agendamento, AgendamentoResponse>()
            .ForMember(dest => dest.Paciente.Telefones, opt => opt.MapFrom(src => src.Paciente.Telefones))
            .ForMember(dest => dest.Medico.Telefones, opt => opt.MapFrom(src => src.Medico.Telefones)).ReverseMap();
        CreateMap<Agendamento, AdicionarAgendamentoRequest>().ReverseMap();
        CreateMap<Agendamento, AtualizarAgendamentoResquet>().ReverseMap();
    }
}
