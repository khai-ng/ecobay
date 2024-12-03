using Core.Entities;

namespace Core.EntityFramework.Entities
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
