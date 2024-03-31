using Core.Events;

namespace Core.Contract
{
    public class HelloEvent : IntergrationEvent
    {
        public HelloEvent(string message) { Message = message; }
        public string Message { get; set; }
    }
}
