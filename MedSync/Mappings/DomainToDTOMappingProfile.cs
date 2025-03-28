using AutoMapper;
using MedSync.Application.Responses;
using MedSync.Domain.Entities;
using static MedSync.Application.Requests.EnderecoRequest;
using static MedSync.Application.Requests.PessoaRequest;

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
    }
}
