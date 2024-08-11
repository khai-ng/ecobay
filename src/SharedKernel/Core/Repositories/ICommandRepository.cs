using Core.SharedKernel;

namespace Core.Repository
{
    public interface ICommandRepository<TEntity, TKey>
        where TEntity : AggregateRoot<TKey>
    {
        /// <summary>
        /// Tracking givens entity. Effecting after <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)" /> called
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// Tracking givens entity. Effecting after <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)" /> called
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// Tracking givens entity. Effecting after <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)" /> called
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);

        /// <summary>
        /// Tracking givens entities. Effecting after <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)" /> called
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Tracking givens entities. Effecting after <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)" /> called
        /// </summary>
        /// <param name="entities"></param>
        void UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Tracking givens entities. Effecting after <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)" /> called
        /// </summary>
        /// <param name="entities"></param>
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
