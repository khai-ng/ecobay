using Core.SharedKernel;

namespace Core.EntityFramework.ServiceDefault
{
    /// <summary>
    /// Base aggreate root class with <see cref="Guid"/> type Id
    /// </summary>
    public abstract class AggregateRoot : AggregateRoot<Guid>
    { }

}
