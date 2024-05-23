using Core.SharedKernel;
using MongoDB.Driver;

namespace Core.MongoDB.Repository
{
    public interface IQueryRepository<TModel, TKey> : Core.Repository.IQueryRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        IMongoCollection<TModel> DbSet { get; }

    }
}
