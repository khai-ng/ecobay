using Core.SharedKernel;

namespace Core.Repository
{
    public interface ICommandRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        /// <summary>
        /// Tracking givens entities. Effecting after <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)" /> called
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<TModel> entities);
        /// <summary>
        /// Tracking givens entities. Effecting after <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)" /> called
        /// </summary>
        /// <param name="entities"></param>
        void UpdateRange(IEnumerable<TModel> entities);
        /// <summary>
        /// Tracking givens entities. Effecting after <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)" /> called
        /// </summary>
        /// <param name="entities"></param>
        void RemoveRange(IEnumerable<TModel> entities);
    }
}
