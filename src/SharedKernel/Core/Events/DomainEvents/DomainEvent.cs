namespace Core.Events.DomainEvents
{

    public abstract record DomainEvent<TKey>(TKey AggregateId) : IDomainEvent<TKey>
    {
        public DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
    }
}
