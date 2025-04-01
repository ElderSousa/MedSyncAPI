using AutoMapper;
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
            .ForPath(dest => dest.Paciente.Telefones, opt => opt.MapFrom(src => src.Paciente.Telefones))
            .ForPath(dest => dest.Medico.Telefones, opt => opt.MapFrom(src => src.Medico.Telefones))
            .ForPath(dest => dest.Medico.Pessoa, opt => opt.MapFrom(src => src.Medico.Pessoa))
            .ForPath(dest => dest.Paciente.Pessoa, opt => opt.MapFrom(src => src.Paciente.Pessoa))
            .ForPath(dest => dest.Paciente.Endereco, opt => opt.MapFrom(src => src.Paciente.Endereco))
            .ForPath(dest => dest.Paciente, opt => opt.MapFrom(src => src.Paciente))
            .ForPath(dest => dest.Medico, opt => opt.MapFrom(src => src.Medico)).ReverseMap();
        CreateMap<Agendamento, AdicionarAgendamentoRequest>().ReverseMap();
        CreateMap<Agendamento, AtualizarAgendamentoResquet>().ReverseMap();
    }
}
