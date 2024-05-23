namespace Core.SharedKernel
{
    public abstract class BaseEntity<TKey>
    {
        public BaseEntity(TKey id) => Id = id;
        public TKey Id { get; protected set; }
    }
}
