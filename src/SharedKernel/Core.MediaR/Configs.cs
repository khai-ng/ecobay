using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.MediaR
{
    public static class Configs
    {
        public static IServiceCollection AddMediatRDefaults(this IServiceCollection services) 
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            });

            return services;
        }
    }
}
