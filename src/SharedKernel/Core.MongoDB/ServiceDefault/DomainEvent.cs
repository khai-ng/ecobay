using Core.SharedKernel;
using MongoDB.Bson;

namespace Core.MongoDB.ServiceDefault
{
    public abstract class DomainEvent : DomainEvent<ObjectId>
    {
        protected DomainEvent() : base(ObjectId.GenerateNewId()) { }
        protected DomainEvent(ObjectId id) : base(id)
        {
        }
    }
}
