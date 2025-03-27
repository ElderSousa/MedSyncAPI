﻿using MedSync.Domain.Interfaces;
using MedSync.Infrastructure.Repositories;
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