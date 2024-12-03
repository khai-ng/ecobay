using Core.Events.DomainEvents;

namespace Core.EntityFramework.Entities
{
    public interface IDomainEventHandler<TModel> : IDomainEventHandler<TModel, Guid>
        where TModel : DomainEvent<Guid>
    { }
}
