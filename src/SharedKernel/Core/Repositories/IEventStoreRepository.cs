using Core.Entities;

namespace Core.Repositories
{
    /// <summary>
    /// IEventStoreRepository with default <see cref="Guid"/> TKey
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEventStoreRepository<TEntity> : IEventStoreRepository<TEntity, Guid>
        where TEntity : AggregateRoot<Guid>
    { }

    public interface IEventStoreRepository<TEntity, TKey>
        where TEntity : AggregateRoot<TKey>
        where TKey : struct
    {
        Task<TEntity?> FindAsync(TKey id, CancellationToken ct = default);
        Task<long> AddAsync(TKey id, TEntity aggregate, CancellationToken ct = default);
        Task<long> UpdateAsync(TKey id, TEntity aggregate, long? expectedVersion = null, CancellationToken ct = default);
    }
}
