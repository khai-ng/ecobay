using Core.Aggregate;

namespace Core.SharedKernel
{
    public interface IRepository<TModel, TKey>
        where TModel : IAggregateRoot<TKey>
    {
        IUnitOfWork UnitOfWork { get; }
    }

    public interface IRepository<TModel> : IRepository<TModel, Ulid>
        where TModel : IAggregateRoot<Ulid>
    {
    }
}
