using Core.Events.DomainEvents;

namespace Core.EntityFramework.Entities
{
    public interface IDomainEvent : IDomainEvent<Guid> { }
}
