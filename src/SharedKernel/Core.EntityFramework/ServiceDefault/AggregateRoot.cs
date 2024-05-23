using Core.SharedKernel;

namespace Core.EntityFramework.ServiceDefault
{
    public abstract class AggregateRoot : AggregateRoot<Ulid>
    {
        protected AggregateRoot() : base(Ulid.NewUlid()) { }
        protected AggregateRoot(Ulid id) : base(id)
        { }
    }

}
