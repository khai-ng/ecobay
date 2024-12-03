using Core.Entities;
using MongoDB.Bson;

namespace Core.MongoDB.Repository
{
    public interface IQueryRepository<TModel> : IQueryRepository<TModel, ObjectId>
        where TModel : AggregateRoot<ObjectId>
    { }

    public interface IQueryRepository<TModel, TKey> : Core.Repositories.IQueryRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
    }
}
