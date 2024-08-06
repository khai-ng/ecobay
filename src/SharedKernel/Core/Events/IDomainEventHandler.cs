using Core.Events;
using MediatR;

namespace Core.IntegrationEvents
{
    public interface IDomainEventHandler<TModel, TKey> : INotificationHandler<TModel>
        where TModel : DomainEvent<TKey>
    { }
}
