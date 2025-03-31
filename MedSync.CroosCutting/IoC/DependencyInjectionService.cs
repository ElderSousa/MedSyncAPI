using MedSync.Application.Interfaces;
using MedSync.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MedSync.CrossCutting.IoC;

public static class DependencyInjectionService
{
    public static IServiceCollection InjectService(this IServiceCollection services)
    {
        services.AddScoped<IPessoaService, PessoaService>();
        services.AddScoped<IEnderecoService, EnderecoService>();
        services.AddScoped<ITelefoneService, TelefoneService>();
        services.AddScoped<IMedicoService, MedicoService>();
        services.AddScoped<IPacienteService, PacienteService>();
        services.AddScoped<IAgendamentoSevice, AgendamentoService>();

        return services;
    }


}