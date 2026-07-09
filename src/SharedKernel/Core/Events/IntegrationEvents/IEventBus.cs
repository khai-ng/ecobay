using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Reflection;

namespace Core.IntegrationEvents.IntegrationEvents
{
    public interface IEventBus
    {
        Task<bool> PublishAsync(IntegrationEvent @event, CancellationToken ct = default);
    }

    public class EventBus : IEventBus
    {
        private static readonly ConcurrentDictionary<Type, (Type HandlerType, MethodInfo Method)> _handlers = new();
        
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventBus> _logger;

        public EventBus(IServiceProvider serviceProvider, ILogger<EventBus> logger)
		{
			_serviceProvider = serviceProvider;
			_logger = logger;
		}

		public async Task<bool> PublishAsync(IntegrationEvent @event, CancellationToken ct = default)
        {
            var eventType = @event.GetType();

            var (handlerType, methodInfo) = _handlers.GetOrAdd(@event.GetType(), type =>
            {
                var constructedHandlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(type);
                var method = constructedHandlerType.GetMethod(nameof(IIntegrationEventHandler<IntegrationEvent>.HandleAsync));
                return (constructedHandlerType, method!);
            });

			var handler = (IIntegrationEventHandler<IntegrationEvent>)_serviceProvider.GetService(handlerType);
            
            if (handler == null)
            {
                _logger.LogWarning("Event bus not found {Event} handler", eventType.Name);
                return false;
            }

            await handler.HandleAsync(@event, ct).ConfigureAwait(false);
            //var task = methodInfo.Invoke(handler, new object[] { @event, ct }) as Task;
            //if (task is null) return false;

            //await task.ConfigureAwait(false);
            return true;
        }
    }
}
