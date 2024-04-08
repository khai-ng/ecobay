using Core.Aggregate;

namespace Core.SharedKernel
{
    public interface IRepository<TModel, TKey> : IAggregateRoot<TKey>
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
