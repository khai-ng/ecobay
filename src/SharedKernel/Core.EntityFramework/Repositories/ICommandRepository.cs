using Core.Entities;
using EFCore.BulkExtensions;

namespace Core.EntityFramework.Repositories
{
    public interface ICommandRepository<TModel> : ICommandRepository<TModel, Guid>
        where TModel : AggregateRoot<Guid>
    { }

    public interface ICommandRepository<TModel, TKey> : Core.Repositories.ICommandRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        Task BulkAddAsync(IEnumerable<TModel> entities, BulkConfig? bulkConfig = null);
        Task BulkUpdateAsync(IEnumerable<TModel> entities, BulkConfig? bulkConfig = null);
        Task BulkDeleteAsync(IEnumerable<TModel> entities, BulkConfig? bulkConfig = null);

    }
}
