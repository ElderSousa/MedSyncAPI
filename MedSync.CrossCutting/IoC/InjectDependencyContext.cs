using MedSync.Application.Mappings;
using MedSync.CrossCutting.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MedSync.CrossCutting.IoC
{
    public static class InjectDependencyContext
    {
        public static IServiceCollection InjectDependency(this IServiceCollection services)
        {
            services.InjectDataBase();
            services.InjectRepository();
            services.InjectService();
            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
