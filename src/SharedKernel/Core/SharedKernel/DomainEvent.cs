using MediatR;

namespace Core.SharedKernel
{
    public abstract class DomainEvent: DomainEvent<Ulid> { }

    public abstract class DomainEvent<TKey> : INotification
    {
        public DomainEvent() { }
        public DomainEvent(TKey id) => Id = id;
        public TKey Id { get; protected set; }
        public long Version { get; set; }

    }
}
