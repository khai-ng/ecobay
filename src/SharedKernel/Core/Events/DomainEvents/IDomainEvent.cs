using MediatR;

namespace Core.Events.DomainEvents
{
    public interface IDomainEvent<TKey> : INotification
    {
        TKey AggregateId { get; }
        //long AggregateVersion { get; }
        DateTime CreatedDate { get; }
    }
}
