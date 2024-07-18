using Core.SharedKernel;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace Core.EntityFramework.Repository
{
    public abstract class Repository<TModel> : Repository<TModel, Ulid>
        where TModel : AggregateRoot<Ulid>
    {
        protected Repository(DbContext context) : base(context)
        {
        }
    }

    public abstract class Repository<TModel, TKey> :
        IRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        internal readonly DbContext _context;
        internal DbSet<TModel> _entity => _context.Set<TModel>();
        protected Repository(DbContext context)
        {
            _context = context;
        }

        public IQueryable<TModel> Collection => _entity;

        public async Task<IEnumerable<TModel>> GetAllAsync()
            => await _entity.ToListAsync();

        public async Task<TModel?> FindAsync(TKey id)
            => await _entity.SingleOrDefaultAsync(x => x.Id.Equals(id));

        public void AddRange(IEnumerable<TModel> entities)
            => _entity.AddRange(entities);

        public void UpdateRange(IEnumerable<TModel> entities)
            => _entity.UpdateRange(entities);

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
