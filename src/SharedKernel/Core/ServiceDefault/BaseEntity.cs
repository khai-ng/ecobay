using Core.SharedKernel;

namespace Core.ServiceDefault
{
    public abstract class BaseEntity : IEntity
    {
        public BaseEntity() { }
        public BaseEntity(Ulid id) => Id = id;
        public Ulid Id { get; protected set; }
    }
}
