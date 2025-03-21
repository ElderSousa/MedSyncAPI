using Catalogo.Application.Interfaces;
using Catalogo.Application.Mappings;
using Catalogo.Application.Services;
using Catalogo.Domain.Interfaces;
using Catalogo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MySql.Data.MySqlClient;

namespace Catalogo.CrossCutting.IoC;

public static class DependencyInjectionAPI
{
    public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services,
        IConfiguration configuration)
    {
       /* services.AddDbContext<ApplicationDbContext>(options =>
           options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                 new MySqlServerVersion(new Version(8, 0, 29))));
       */

        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<ICategoriaService, CategoriaService>();

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
