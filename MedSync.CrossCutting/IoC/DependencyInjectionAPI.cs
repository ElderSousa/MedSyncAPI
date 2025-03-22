using MedSync.Application.Interfaces;
using MedSync.Application.Mappings;
using MedSync.Application.Services;
using MedSync.Domain.Interfaces;
using MedSync.Infrastructure.Repositories;
using MedSync.Infrastructure.Repositories.Scripts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MySql.Data.MySqlClient;

namespace MedSync.CrossCutting.IoC;

public static class DependencyInjectionAPI
{
    public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services)
    {
     
        services.AddScoped<IPessoaRepository, PessoaRepository>();
        services.AddScoped<IPessoaService, PessoaService>();
       

        services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

        services.InjectDataBase();

        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }

    private static IServiceCollection InjectDataBase(this IServiceCollection services)
    {
        #region MySQL
        var hostName = Environment.GetEnvironmentVariable("MYSQL_SERVER_MEDSYNC") ?? "";
        var dataBaseName = Environment.GetEnvironmentVariable("MYSQL_DB_MEDSYNC") ?? "";
        var port = Environment.GetEnvironmentVariable("MYSQL_PORT_MEDSYNC") ?? "";
        var user = Environment.GetEnvironmentVariable("MYSQL_USER_MEDSYNC") ?? "";
        var pass = Environment.GetEnvironmentVariable("MYSQL_PASS_MEDSYNC") ?? "";

        services.AddScoped(x => new MySqlConnection($"Server={hostName};Port={port};Database={dataBaseName};Uid={user};Pwd={pass};"));

        return services;
        #endregion
    }
}
