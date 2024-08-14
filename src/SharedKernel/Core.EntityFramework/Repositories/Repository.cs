using Core.SharedKernel;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace Core.EntityFramework.Repository
{
    public abstract class Repository<TModel> : Repository<TModel, Guid>
        where TModel : AggregateRoot<Guid>
    {
        protected Repository(DbContext context) : base(context)
        {
        }
    }

    public abstract class Repository<TEntity, TKey> :
        IRepository<TEntity, TKey>
        where TEntity : AggregateRoot<TKey>
    {
        internal readonly DbContext _context;
        internal DbSet<TEntity> _entity => _context.Set<TEntity>();
        protected Repository(DbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> Collection => _entity;

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _entity.ToListAsync();

        public async Task<TEntity?> FindAsync(TKey id)
            => await _entity.SingleOrDefaultAsync(x => x.Id.Equals(id));

        public void Add(TEntity entity)
            => _entity.Add(entity);

        public void Update(TEntity entity)
            => _entity.Update(entity);

        public void Remove(TEntity entity)
            => _entity.Remove(entity);

        public void AddRange(IEnumerable<TEntity> entities)
            => _entity.AddRange(entities);

        public void UpdateRange(IEnumerable<TEntity> entities)
            => _entity.UpdateRange(entities);

        public void RemoveRange(IEnumerable<TEntity> entities)
            => _entity.RemoveRange(entities);

        public async Task BulkAddAsync(IEnumerable<TEntity> entities, BulkConfig? bulkConfig = null)
            => await _context.BulkInsertAsync(entities, bulkConfig);

        public async Task BulkUpdateAsync(IEnumerable<TEntity> entities, BulkConfig? bulkConfig = null)
            => await _context.BulkUpdateAsync(entities, bulkConfig);

        public async Task BulkDeleteAsync(IEnumerable<TEntity> entities, BulkConfig? bulkConfig = null)
            => await _context.BulkDeleteAsync(entities, bulkConfig);
    }
}
