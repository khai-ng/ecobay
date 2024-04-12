using Core.SharedKernel;
using EFCore.BulkExtensions;

namespace Core.ServiceDefault
{
    public interface ICommonCommandRepository<TModel>: ICommonCommandRepository<TModel, Ulid>
        where TModel : BaseAggregateRoot<Ulid>
    { }

    public interface ICommonCommandRepository<TModel, TKey>: IRepository<TModel, TKey>
        where TModel : BaseAggregateRoot<TKey>
    {
        void AddRange(IEnumerable<TModel> entities);
        void UpdateRange(IEnumerable<TModel> entities);
        void RemoveRange(IEnumerable<TModel> entities);
        Task BulkAddAsync(IEnumerable<TModel> entities, BulkConfig? bulkConfig = null);
        Task BulkUpdateAsync(IEnumerable<TModel> entities, BulkConfig? bulkConfig = null);
        Task BulkDeleteAsync(IEnumerable<TModel> entities, BulkConfig? bulkConfig = null);
    }
}
