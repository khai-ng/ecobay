using Core.IntegrationEvents;
using MongoDB.Bson;

namespace Core.MongoDB.ServiceDefault
{
    public interface IDomainEvent : IDomainEvent<ObjectId> { }
}
