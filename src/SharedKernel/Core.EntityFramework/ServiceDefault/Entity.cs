using Core.SharedKernel;

namespace Core.EntityFramework.ServiceDefault
{
    public abstract class Entity : BaseEntity<Ulid>
    {
        protected Entity() : base(Ulid.NewUlid())
        { }

        protected Entity(Ulid id) : base(id)
        { }
    }
}
