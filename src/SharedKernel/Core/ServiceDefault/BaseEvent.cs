using Core.SharedKernel;

namespace Core.ServiceDefault
{
    public abstract class BaseEvent : IEvent
    {
        public BaseEvent() { }
        public BaseEvent(Ulid id) => Id = id;
        public Ulid Id { get; protected set; }
        public long Version { get; set; }

    }
}
