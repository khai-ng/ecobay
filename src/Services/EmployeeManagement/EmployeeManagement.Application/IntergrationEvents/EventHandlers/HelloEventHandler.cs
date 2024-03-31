using Core.Autofac;
using Core.Contract;
using Core.Events;
using Serilog;
using System.Text.Json;

namespace EmployeeManagement.Application.IntergrationEvents.EventHandlers
{
    public sealed class HelloEventHandler : IIntergrationEventHandler<HelloEvent>, ITransient
    {
        private readonly ILogger _logger;
        public HelloEventHandler(ILogger logger)
        {
            _logger = logger;
        }

        public Task Handle(HelloEvent notification, CancellationToken cancellationToken)
        {
            return Task.Run(() => { _logger.Information(JsonSerializer.Serialize(notification)); }); 
        }
    }
}
