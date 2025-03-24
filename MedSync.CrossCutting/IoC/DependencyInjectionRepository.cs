using MedSync.Application.Interfaces;
using MedSync.Application.Mappings;
using MedSync.Application.Services;
using MedSync.Domain.Interfaces;
using MedSync.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MedSync.CrossCutting.IoC;

public static class DependencyInjectionRepository
{
    public static IServiceCollection InjectRepository(this IServiceCollection services)
    {
        services.AddScoped<IPessoaRepository, PessoaRepository>();

        return services;
    }
}
