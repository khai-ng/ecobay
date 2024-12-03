using Core.Events.DomainEvents;

namespace Core.EntityFramework.Entities
{
    /// <summary>
    /// Base domain event class with <see cref="Guid"/> type Id
    /// </summary>
    public abstract class DomainEvent : DomainEvent<Guid>
    {
        protected DomainEvent() : base(Guid.NewGuid()) { }
        protected DomainEvent(Guid aggregate) : base(aggregate)
        {
        }
    }
}
