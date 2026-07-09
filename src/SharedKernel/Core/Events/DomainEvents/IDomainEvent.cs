using Core.Mediator;

namespace Core.Events.DomainEvents
{
    public interface IDomainEvent<TKey> : IRequest
    {
        TKey AggregateId { get; }
		//long AggregateVersion { get; }
		DateTimeOffset CreatedAt { get; }
    }
}
