using Core.Events.DomainEvents;

namespace Core.EntityFramework.ServiceDefault
{
    public interface IDomainEventHandler<TModel> : IDomainEventHandler<TModel, Guid>
        where TModel : DomainEvent<Guid>
    { }
}
