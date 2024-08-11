using Core.Repository;
using Core.SharedKernel;
using MongoDB.Bson;

namespace Core.MongoDB.Repository
{
    public interface ICommandRepository<TModel> : ICommandRepository<TModel, ObjectId>
        where TModel : AggregateRoot<ObjectId>
    { }
}
