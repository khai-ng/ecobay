namespace Core.SharedKernel
{
    public abstract class Entity<TKey>
    {
        public TKey Id { get; set; }
        public Entity(TKey id) => Id = id;
    }
}
