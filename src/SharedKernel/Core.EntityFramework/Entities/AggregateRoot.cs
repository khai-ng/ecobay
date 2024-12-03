using Core.Entities;

namespace Core.EntityFramework.Entities
{
    /// <summary>
    /// Base aggreate root class with <see cref="Guid"/> type Id
    /// </summary>
    public abstract class AggregateRoot : AggregateRoot<Guid>
    { }

}
