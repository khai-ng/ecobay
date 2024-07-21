namespace Core.SharedKernel
{
    public abstract class Entity<TKey>
    {
        public Entity(TKey id) => Id = id;
        public TKey Id { get; protected set; }
    }
}
