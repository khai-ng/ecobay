using MediatR;

namespace Core.SharedKernel
{
    public abstract class DomainEvent : DomainEvent<Ulid>
    {
        protected DomainEvent() : base(Ulid.NewUlid()) { }
        protected DomainEvent(Ulid id) : base(id)
        {
        }
    }

    public abstract class DomainEvent<TKey> : IDomainEvent<TKey>
    {
        protected DomainEvent(TKey id) => Id = id;
        public TKey Id { get; protected set; }
        public long Version { get; protected set; }
    }
}
