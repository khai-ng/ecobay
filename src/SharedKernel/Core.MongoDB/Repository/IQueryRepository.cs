using Core.SharedKernel;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Core.MongoDB.Repository
{
    public interface IQueryRepository<TModel> : IQueryRepository<TModel, ObjectId>
        where TModel : AggregateRoot<ObjectId>
    { }

    public interface IQueryRepository<TModel, TKey> : Core.Repository.IQueryRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        IMongoCollection<TModel> Collection { get; }
    }
}
