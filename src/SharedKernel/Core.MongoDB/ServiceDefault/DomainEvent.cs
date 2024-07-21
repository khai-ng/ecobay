using Core.SharedKernel;
using MongoDB.Bson;

namespace Core.MongoDB.ServiceDefault
{
    /// <summary>
    /// Base domain event class with <see cref="ObjectId"/> type Id
    /// </summary>
    public abstract class DomainEvent : DomainEvent<ObjectId>
    {
        protected DomainEvent() : base(ObjectId.GenerateNewId()) { }
        protected DomainEvent(ObjectId id) : base(id)
        {
        }
    }
}
