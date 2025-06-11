using Microsoft.Extensions.DependencyInjection;

namespace CroosCutting.MS_AuthenticationAutorization.IoC
{
    public static class CorsServiceCollectionExtension
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowCredentials();
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                    policy.WithOrigins("http://localhost:5000"
                       
                    );
                });
            });

            return services;
        }
    }
}
