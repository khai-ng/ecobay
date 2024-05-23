using Core.SharedKernel;
using EFCore.BulkExtensions;

namespace Core.EntityFramework.Repository
{
    public interface ICommandRepository<TModel, TKey> : Core.Repository.ICommandRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        Task BulkAddAsync(IEnumerable<TModel> entities, BulkConfig? bulkConfig = null);
        Task BulkUpdateAsync(IEnumerable<TModel> entities, BulkConfig? bulkConfig = null);
        Task BulkDeleteAsync(IEnumerable<TModel> entities, BulkConfig? bulkConfig = null);

    }
}
