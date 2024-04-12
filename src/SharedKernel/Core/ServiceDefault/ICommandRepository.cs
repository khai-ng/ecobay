using Core.SharedKernel;
using EFCore.BulkExtensions;

namespace Core.ServiceDefault
{
    public interface ICommandRepository<TModel>: ICommandRepository<TModel, Ulid>
        where TModel : AggregateRoot<Ulid>
    { }

    public interface ICommandRepository<TModel, TKey>: IRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        void AddRange(IEnumerable<TModel> entities);
        void UpdateRange(IEnumerable<TModel> entities);
        void RemoveRange(IEnumerable<TModel> entities);
        Task BulkAddAsync(IEnumerable<TModel> entities, BulkConfig? bulkConfig = null);
        Task BulkUpdateAsync(IEnumerable<TModel> entities, BulkConfig? bulkConfig = null);
        Task BulkDeleteAsync(IEnumerable<TModel> entities, BulkConfig? bulkConfig = null);
    }
}
