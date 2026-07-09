using Core.Entities;

namespace Core.EntityFramework.Entities
{
    /// <summary>
    /// Base entity class with <see cref="Guid"/> type Id
    /// </summary>
    public abstract class Entity : Entity<Guid>
    {
        protected Entity() : base(Guid.CreateVersion7())
        { }

        protected Entity(Guid id) : base(id)
        { }
    }
}
