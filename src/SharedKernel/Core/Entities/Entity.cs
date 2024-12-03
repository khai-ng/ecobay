namespace Core.Entities
{
    public abstract class Entity<TKey>
    {
        public TKey Id { get; set; }

        protected Entity() { }
        protected Entity(TKey id) => Id = id;
    }
}
