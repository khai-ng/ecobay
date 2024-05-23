using Core.SharedKernel;

namespace Core.EntityFramework.ServiceDefault
{
    public abstract class DomainEvent : DomainEvent<Ulid>
    {
        protected DomainEvent() : base(Ulid.NewUlid()) { }
        protected DomainEvent(Ulid id) : base(id)
        {
        }
    }
}
