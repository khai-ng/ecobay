using Core.IntegrationEvents;

namespace Core.Events
{

    public abstract class DomainEvent<TKey> : IDomainEvent<TKey>
    {
        protected DomainEvent(TKey id) => Id = id;
        public TKey Id { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
    }
}
