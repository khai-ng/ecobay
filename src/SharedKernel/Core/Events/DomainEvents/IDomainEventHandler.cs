using MediatR;

namespace Core.Events.DomainEvents
{
    public interface IDomainEventHandler<TModel, TKey> : INotificationHandler<TModel>
        where TModel : DomainEvent<TKey>
    { }
}
