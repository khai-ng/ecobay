using Core.SharedKernel;
using MongoDB.Bson;

namespace Core.MongoDB.ServiceDefault
{
    public interface IDomainEventHandler<TModel> : IDomainEventHandler<TModel, ObjectId>
        where TModel : DomainEvent<ObjectId>
    { }
}
