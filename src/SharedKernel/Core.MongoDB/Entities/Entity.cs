using Core.SharedKernel;
using MongoDB.Bson;

namespace Core.MongoDB.ServiceDefault
{
    /// <summary>
    /// Base entity class with <see cref="ObjectId"/> type Id
    /// </summary>
    public abstract class Entity : Entity<ObjectId>
    {
        protected Entity() : base(ObjectId.GenerateNewId())
        { }

        protected Entity(ObjectId id) : base(id)
        { }
    }
}
