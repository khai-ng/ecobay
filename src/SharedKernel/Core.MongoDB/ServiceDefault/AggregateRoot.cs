using Core.SharedKernel;
using MongoDB.Bson;

namespace Core.MongoDB.ServiceDefault
{
    public abstract class AggregateRoot : AggregateRoot<ObjectId>
    {
        protected AggregateRoot() : base(ObjectId.GenerateNewId()) { }
        protected AggregateRoot(ObjectId id) : base(id)
        { }
    }

}
