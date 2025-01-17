using Core.Entities;
using EFCore.BulkExtensions;

namespace Core.EntityFramework.Repositories
{
    public interface ICommandRepository<TEntity> : ICommandRepository<TEntity, Guid>
        where TEntity : AggregateRoot<Guid>
    { }

    public interface ICommandRepository<TEntity, TKey> : Core.Repositories.ICommandRepository<TEntity, TKey>
        where TEntity : AggregateRoot<TKey>
    {
        Task BulkAddAsync(IEnumerable<TEntity> entities, BulkConfig? bulkConfig = null);
        Task BulkUpdateAsync(IEnumerable<TEntity> entities, BulkConfig? bulkConfig = null);
        Task BulkDeleteAsync(IEnumerable<TEntity> entities, BulkConfig? bulkConfig = null);

    }
}
