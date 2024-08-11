using Core.SharedKernel;
using Microsoft.Extensions.DependencyInjection;

namespace Core.EntityFramework.Context
{
    public static class EntityExtension
    {
        public static IServiceCollection AddEFServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
