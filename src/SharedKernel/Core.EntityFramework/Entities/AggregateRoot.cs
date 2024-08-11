using Core.SharedKernel;

namespace Core.EntityFramework.ServiceDefault
{
    /// <summary>
    /// Base aggreate root class with <see cref="Ulid"/> type Id
    /// </summary>
    public abstract class AggregateRoot : AggregateRoot<Ulid>
    {
        protected AggregateRoot() : base(Ulid.NewUlid()) { }
        protected AggregateRoot(Ulid id) : base(id)
        { }
    }

}
