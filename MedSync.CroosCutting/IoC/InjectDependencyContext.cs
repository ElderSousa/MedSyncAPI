using MedSync.Application.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MedSync.CrossCutting.IoC
{
    public static class InjectDependencyContext
    {
        public static IServiceCollection InjectDependency(this IServiceCollection services)
        {
            services.InjectRepository();
            services.InjectService();
            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}