using Core.SharedKernel;

namespace Core.EntityFramework.ServiceDefault
{
    public interface IDomainEvent : IDomainEvent<Ulid> { }
}
