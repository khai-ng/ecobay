using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Core.IntegrationEvents.IntegrationEvents
{
    public interface IEventBus
    {
        Task<bool> PublishAsync(IntegrationEvent @event, CancellationToken ct = default);
    }

    public class EventBus : IEventBus
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

		public EventBus(IServiceProvider serviceProvider, ILogger logger)
		{
			_serviceProvider = serviceProvider;
			_logger = logger;
		}

		public async Task<bool> PublishAsync(IntegrationEvent @event, CancellationToken ct = default)
        {
            using var scope = _serviceProvider.CreateScope();
            var eventHandleType = typeof(IIntegrationEventHandler<>).MakeGenericType(@event.GetType());
            if (eventHandleType is null) return false;

			var handler = scope.ServiceProvider.GetService(eventHandleType);
            
            if (handler == null)
            {
                _logger
                    .ForContext(typeof(EventBus))
                    .Warning("Event bus not found {Event} handler", @event.GetType().Name);
                return false;
            }

            var methodInfo = eventHandleType.GetMethod(nameof(IIntegrationEventHandler<IntegrationEvent>.HandleAsync));
            var task = methodInfo?.Invoke(handler, new object[] { @event, ct }) as Task;

            if (task is null) return false;

            await task.ConfigureAwait(false);

            return true;
        }
    }
}
