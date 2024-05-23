using Core.SharedKernel;
using MongoDB.Bson;

namespace Core.MongoDB.ServiceDefault
{

    public abstract class Entity : BaseEntity<ObjectId>
    {
        protected Entity() : base(ObjectId.GenerateNewId())
        { }

        protected Entity(ObjectId id) : base(id)
        { }
    }
}
