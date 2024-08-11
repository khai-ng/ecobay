using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Core.IntegrationEvents.IntegrationEvents
{
    public interface IEventBus
    {
        Task PublishAsync(IntegrationEvent evt, CancellationToken ct = default);
    }

    public class EventBus : IEventBus
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

		public EventBus( IServiceProvider serviceProvider, ILogger logger)
		{
			_serviceProvider = serviceProvider;
			_logger = logger;
		}

		public async Task PublishAsync(IntegrationEvent evt, CancellationToken ct = default)
        {
            using var scope = _serviceProvider.CreateScope();
            var eventHandleType = typeof(IIntegrationEventHandler<>).MakeGenericType(evt.GetType());
            if (eventHandleType is null) return;

			var handlers = scope.ServiceProvider.GetServices(eventHandleType);

            foreach (var handler in handlers)
            {
                var methodInfo = eventHandleType.GetMethod(nameof(IIntegrationEventHandler<IntegrationEvent>.HandleAsync));
				var task = methodInfo?.Invoke(handler, new object[] { evt, ct }) as Task;

				if (task is null) return;

				await task.ConfigureAwait(false);
            }
        }
    }
}
