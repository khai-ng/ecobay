using Core.Entities;

namespace Core.Repositories
{
    public interface IRepository<TEntity, TKey> :
        IQueryRepository<TEntity, TKey>,
        ICommandRepository<TEntity, TKey>
        where TEntity : AggregateRoot<TKey>
    { }
}
