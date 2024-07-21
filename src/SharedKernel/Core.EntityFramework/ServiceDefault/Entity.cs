using Core.SharedKernel;

namespace Core.EntityFramework.ServiceDefault
{
    /// <summary>
    /// Base entity class with <see cref="Ulid"/> type Id
    /// </summary>
    public abstract class Entity : Entity<Ulid>
    {
        protected Entity() : base(Ulid.NewUlid())
        { }

        protected Entity(Ulid id) : base(id)
        { }
    }
}
