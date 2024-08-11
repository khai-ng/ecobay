using Core.Events.DomainEvents;
using MongoDB.Bson;

namespace Core.MongoDB.ServiceDefault
{
    public interface IDomainEvent : IDomainEvent<ObjectId> { }
}
