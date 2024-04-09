using Core.SharedKernel;

namespace Core.Aggregate
{
    public interface IAggregateRoot<out TKey>: IEntity<TKey>
    {
        long Version { get; }
        IReadOnlyCollection<IEvent<TKey>> Events { get; }
        void ClearEvents();
    }

    public interface IAggregateRoot: IAggregateRoot<Ulid> { }
}
