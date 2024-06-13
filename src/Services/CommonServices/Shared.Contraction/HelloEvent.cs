using Core.IntegrationEvents;

namespace Core.Contract
{
    public class HelloEvent : IntegrationEvent
    {
        public HelloEvent(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}
