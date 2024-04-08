using Core.ServiceDefault;

namespace Core.Contract
{
    public class HelloEvent : BaseEvent
    {
        public HelloEvent(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}
