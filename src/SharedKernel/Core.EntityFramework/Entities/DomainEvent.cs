using Core.Events.DomainEvents;

namespace Core.EntityFramework.Entities
{
    /// <summary>
    /// Base domain event class with <see cref="Guid"/> type Id
    /// </summary>
    public abstract record DomainEvent : DomainEvent<Guid>
    {
        protected DomainEvent(Guid aggregate) : base(aggregate)
        { }
    }
}
