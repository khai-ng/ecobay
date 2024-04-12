using Core.Aggregate;
using Core.SharedKernel;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace Core.ServiceDefault
{
    public abstract class CommonCommandRepository<TModel> : CommonCommandRepository<TModel, Ulid>
        where TModel : class, IAggregateRoot<Ulid>
    {
        protected CommonCommandRepository(DbContext context) : base(context)
        {
        }
    }

    public abstract class CommonCommandRepository<TModel, TKey> : ICommonCommandRepository<TModel, TKey>
        where TModel : class, IAggregateRoot<TKey>
    {
        internal readonly DbContext _context;
        internal DbSet<TModel> _entity => _context.Set<TModel>();
        protected CommonCommandRepository(DbContext context)
        {
                _context = context;
        }

        /// <summary>
        /// Tracking givens entities. Effecting after <see cref="IUnitOfWork.SaveAsync(CancellationToken)" /> called
        /// </summary>
        /// <param name="entities"></param>
        public void AddRange(IEnumerable<TModel> entities)
            => _entity.AddRange(entities);

        /// <summary>
        /// Tracking givens entities. Effecting after <see cref="IUnitOfWork.SaveAsync(CancellationToken)" /> called
        /// </summary>
        /// <param name="entities"></param>
        public void UpdateRange(IEnumerable<TModel> entities)
            => _entity.UpdateRange(entities);

        /// <summary>
        /// Tracking givens entities. Effecting after <see cref="IUnitOfWork.SaveAsync(CancellationToken)" /> called
        /// </summary>
        /// <param name="entities"></param>
        public void RemoveRange(IEnumerable<TModel> entities)
            => _entity.RemoveRange(entities);

        public async Task BulkAddAsync(IEnumerable<TModel> entities, BulkConfig? bulkConfig = null)
            => await _context.BulkInsertAsync(entities, bulkConfig);

        public async Task BulkUpdateAsync(IEnumerable<TModel> entities, BulkConfig? bulkConfig = null)
            => await _context.BulkUpdateAsync(entities, bulkConfig);

        public async Task BulkDeleteAsync(IEnumerable<TModel> entities, BulkConfig? bulkConfig = null)
            => await _context.BulkDeleteAsync(entities, bulkConfig);
    }
}
