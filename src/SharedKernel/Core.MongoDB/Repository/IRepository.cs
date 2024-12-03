using Core.Entities;
using Core.Repositories;
using MongoDB.Bson;

namespace Core.MongoDB.Repository
{
    public interface IRepository<TModel> : IRepository<TModel, ObjectId>
        where TModel : AggregateRoot<ObjectId>
    { }

    public interface IRepository<TModel, TKey> :
        IQueryRepository<TModel, TKey>,
        ICommandRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    { }
}
