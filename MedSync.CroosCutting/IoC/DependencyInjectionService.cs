using MedSync.Application.Interfaces;
using MedSync.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MedSync.CrossCutting.IoC;

public static class DependencyInjectionService
{
    public static IServiceCollection InjectService(this IServiceCollection services)
    {
        services.AddScoped<IPessoaService, PessoaService>();

        return services;
    }


}