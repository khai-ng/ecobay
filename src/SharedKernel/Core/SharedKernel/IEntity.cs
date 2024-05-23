namespace Core.SharedKernel
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}
