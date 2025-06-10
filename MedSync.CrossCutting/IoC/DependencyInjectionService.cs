using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MedSync.Application.Interfaces;
using MedSync.Application.Services;
using MedSync.Application.Validation;


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
        services.AddScoped<IAgendaSevice, AgendaService>();
        services.AddScoped<IHorarioService, HorarioService>();
        services.AddScoped<IAgendamentoService, AgendamentoService>();
        services.AddValidatorsFromAssemblyContaining<AgendamentoValidation>();//Única chamada o Validator varre todo o assembly para injetar todas a validações.

        return services;
    }


}