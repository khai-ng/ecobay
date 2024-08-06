using Core.Autofac;
using Core.IntegrationEvents.IntegrationEvents;

namespace Ordering.API.Application.IntergrationEvents.HelloEvent
{
	public class HelloEventHandler : IIntegrationEventHandler<HelloEvent>, ITransient
	{
		private readonly Serilog.ILogger _logger;

		public HelloEventHandler(Serilog.ILogger logger)
		{
			_logger = logger;
		}

		public Task HandleAsync(HelloEvent @event, CancellationToken cancellationToken = default)
		{
			_logger.Information("Handling message: {Message}", nameof(HelloEvent));
			return Task.CompletedTask;
		}
	}
}
