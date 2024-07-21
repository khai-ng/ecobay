using Core.SharedKernel;

namespace Core.EntityFramework.ServiceDefault
{
    /// <summary>
    /// Base domain event class with <see cref="Ulid"/> type Id
    /// </summary>
    public abstract class DomainEvent : DomainEvent<Ulid>
    {
        protected DomainEvent() : base(Ulid.NewUlid()) { }
        protected DomainEvent(Ulid id) : base(id)
        {
        }
    }
}
