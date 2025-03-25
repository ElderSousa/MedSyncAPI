﻿using MedSync.Infrastructure.DbContextRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace MedSync.CrossCutting.Data;

public static class DataBaseInjection
{
    public static IServiceCollection InjectDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
           options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                 new MySqlServerVersion(new Version(8, 0, 29))));

        #region MYSQL
        var hostName = Environment.GetEnvironmentVariable("MYSQL_SERVER_MEDSYNC") ?? "";
        var dataBaseName = Environment.GetEnvironmentVariable("MYSQL_DB_MEDSYNC") ?? "";
        var port = Environment.GetEnvironmentVariable("MYSQL_PORT_MEDSYNC") ?? "";
        var user = Environment.GetEnvironmentVariable("MYSQL_USER_MEDSYNC") ?? "";
        var pass = Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "";

        services.AddScoped(x => new MySqlConnection($"Server={hostName};Port={port};Database={dataBaseName};Uid={user};Pwd={pass};"));

        return services;
        #endregion
    }
}
