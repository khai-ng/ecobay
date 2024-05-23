using Core.SharedKernel;
using MongoDB.Bson;

namespace Core.MongoDB.ServiceDefault
{
    public interface IDomainEvent : IDomainEvent<ObjectId> { }
}
