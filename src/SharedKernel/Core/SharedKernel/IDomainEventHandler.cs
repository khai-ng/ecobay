using MediatR;

namespace Core.SharedKernel
{
    public interface IDomainEventHandler<TModel, TKey> : INotificationHandler<TModel>
        where TModel : DomainEvent<TKey>
    { }

    public interface IDomainEventHandler<TModel> : IDomainEventHandler<TModel, Ulid>
        where TModel : DomainEvent<Ulid>
    { }
}
