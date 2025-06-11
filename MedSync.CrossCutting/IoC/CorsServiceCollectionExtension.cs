using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CroosCutting.MS_AuthenticationAutorization.IoC
{
    public static class CorsServiceCollectionExtension
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("Development", builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowCredentials();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                        builder.WithOrigins("http://localhost:8080"
                        );
                    });
                });
            }
            /*else
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("Production", builder =>
                    {
                        builder.WithOrigins("")
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
                });
            }*/

            return services;
        }
    }
}
