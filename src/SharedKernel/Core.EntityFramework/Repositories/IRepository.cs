using Core.Entities;
using Core.Repositories;

namespace Core.EntityFramework.Repositories
{
    public interface IRepository<TEntity> : IRepository<TEntity, Guid>
        where TEntity : AggregateRoot<Guid>
    { }

    public interface IRepository<TEntity, TKey> :
        IQueryRepository<TEntity, TKey>,
        ICommandRepository<TEntity, TKey>
        where TEntity : AggregateRoot<TKey>
    { }
}
