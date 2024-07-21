using Core.SharedKernel;
using MongoDB.Bson;

namespace Core.MongoDB.ServiceDefault
{
    /// <summary>
    /// Base aggreate root class with <see cref="ObjectId"/> type Id
    /// </summary>
    public abstract class AggregateRoot : AggregateRoot<ObjectId>
    {
        protected AggregateRoot() : base(ObjectId.GenerateNewId()) { }
        protected AggregateRoot(ObjectId id) : base(id)
        { }
    }

}
