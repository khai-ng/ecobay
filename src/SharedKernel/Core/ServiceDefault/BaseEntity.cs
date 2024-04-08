using Core.SharedKernel;

namespace Core.ServiceDefault
{
    public abstract class BaseEntity : IEntity
    {
        public BaseEntity() { }
        public BaseEntity(Guid id) => Id = id;
        public Guid Id { get; protected set; }
    }
}
