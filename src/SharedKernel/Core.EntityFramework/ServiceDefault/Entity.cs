using Core.SharedKernel;

namespace Core.EntityFramework.ServiceDefault
{
    public abstract class Entity : Entity<Ulid>
    {
        protected Entity() : base(Ulid.NewUlid())
        { }

        protected Entity(Ulid id) : base(id)
        { }
    }
}
