using Core.Events;
using Core.IntegrationEvents;

namespace Core.EntityFramework.ServiceDefault
{
    public interface IDomainEventHandler<TModel> : IDomainEventHandler<TModel, Ulid>
        where TModel : DomainEvent<Ulid>
    { }
}
