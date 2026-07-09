using Microsoft.Extensions.DependencyInjection;

namespace Core.Mediator
{
    public static class Configs
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddTransient(typeof(RequestHandlerBase<,>));
            services.AddTransient<IMediator, Mediator>();

            return services;
        }
    }
}
