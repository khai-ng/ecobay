using Core.SharedKernel;

namespace Core.EntityFramework.ServiceDefault
{
    /// <summary>
    /// Base entity class with <see cref="Guid"/> type Id
    /// </summary>
    public abstract class Entity : Entity<Guid>
    {
        protected Entity() : base(Guid.NewGuid())
        { }

        protected Entity(Guid id) : base(id)
        { }
    }
}
