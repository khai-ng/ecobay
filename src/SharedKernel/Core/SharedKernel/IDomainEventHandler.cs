using MediatR;

namespace Core.SharedKernel
{
    public interface IDomainEventHandler<TModel, TKey> : INotificationHandler<TModel>
        where TModel : DomainEvent<TKey>
    { }
}
