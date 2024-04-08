using Core.SharedKernel;

namespace Core.ServiceDefault
{
    public abstract class BaseEvent : IEvent
    {
        public BaseEvent() { }
        public BaseEvent(Guid id) => Id = id;
        public Guid Id { get; protected set; }
        public long Version { get; set; }

    }
}
