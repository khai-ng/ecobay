using Core.Events.DomainEvents;

namespace Core.EntityFramework.ServiceDefault
{
    public interface IDomainEvent : IDomainEvent<Guid> { }
}
