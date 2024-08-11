using Core.SharedKernel;

namespace Core.Events.EventStore
{
    /// <summary>
    /// IEventStoreRepository with default <see cref="Ulid"/> TKey
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEventStoreRepository<TEntity>: IEventStoreRepository<TEntity, Ulid>
        where TEntity : AggregateRoot<Ulid>
    { }

    public interface IEventStoreRepository<TEntity, TKey> 
        where TEntity : AggregateRoot<TKey>
    {
        Task<TEntity?> Find(TKey id, CancellationToken ct);
        Task<long> Add(TKey id, TEntity aggregate, CancellationToken ct = default);
        Task<long> Update(TKey id, TEntity aggregate, long? expectedVersion = null, CancellationToken ct = default);
        //Task<long> Delete(TKey id, TEntity aggregate, long? expectedVersion = null, CancellationToken ct = default);
    }
}
