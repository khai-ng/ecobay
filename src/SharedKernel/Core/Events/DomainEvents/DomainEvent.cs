namespace Core.Events.DomainEvents
{
    public abstract record DomainEvent<TKey>(TKey AggregateId) : IDomainEvent<TKey>
    {
        public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.Now;
    }
}
