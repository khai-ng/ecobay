namespace Core.Events.DomainEvents
{

    public abstract class DomainEvent<TKey> : IDomainEvent<TKey>
    {
        protected DomainEvent(TKey aggregateId) => AggregateId = aggregateId;
        public TKey AggregateId { get; protected set; }
        public DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
    }
}
