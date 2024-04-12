using Core.SharedKernel;

namespace Core.Aggregate
{
    public interface IAggregateRoot<TKey>: IEntity<TKey>
    {
        long Version { get; }
        IReadOnlyCollection<IEvent<TKey>> Events { get; }
        void ClearEvents();
    }

    public interface IAggregateRoot: IAggregateRoot<Ulid> { }
}
