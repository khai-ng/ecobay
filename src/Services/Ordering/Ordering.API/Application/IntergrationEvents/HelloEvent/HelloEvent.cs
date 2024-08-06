using Core.IntegrationEvents.IntegrationEvents;

namespace Ordering.API.Application.IntergrationEvents.HelloEvent
{
    public class HelloEvent : IntegrationEvent
    {
        public HelloEvent(string message) { Message = message; }
        public string Message { get; set; } = string.Empty;
    }
}
