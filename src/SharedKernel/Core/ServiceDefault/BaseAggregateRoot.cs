using Core.SharedKernel;

namespace Core.ServiceDefault
{
    public abstract class BaseAggregateRoot : BaseAggregateRoot<Ulid>
    {
        protected BaseAggregateRoot() { }
        protected BaseAggregateRoot(Ulid id) : base(id)
        {
        }
    }
}
