using Core.Entities;
using System.Linq.Expressions;

namespace Core.Repositories
{
    public interface IQueryRepository<TEntity, TKey>
        where TEntity : AggregateRoot<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TDestination>> GetAllAsync<TDestination>(Expression<Func<TEntity, TDestination>> selector);
        Task<TEntity?> FindAsync(TKey id);
        Task<TDestination?> FindAsync<TDestination>(TKey id, Expression<Func<TEntity, TDestination>> selector);
    }
}
