using AutoMapper;
using MedSync.Application.Requests;
using MedSync.Application.Responses;
using MedSync.Domain.Entities;

namespace MedSync.Application.Mappings;

public class DomainToDTOMappingProfile : Profile
{
    public DomainToDTOMappingProfile()
    {
        CreateMap<Pessoa, PessoaRequest>().ReverseMap();
        CreateMap<Pessoa, PessoaResponse>().ReverseMap();
    }
}
