using MongoDB.Bson.Serialization.Attributes;

namespace Core.SharedKernel
{
    public abstract class Entity : BaseEntity<Ulid>
    {
        protected Entity() : base(Ulid.NewUlid())
        { }

        protected Entity(Ulid id) : base(id)
        { }
    }

    public abstract class BaseEntity<TKey>
    {
        public BaseEntity(TKey id) => Id = id;
        [BsonId]
        public TKey Id { get; protected set; }
    }
}
