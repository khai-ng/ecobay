using Core.Entities;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.EntityFramework.Repositories
{
    public abstract class Repository<TEntity> : Repository<TEntity, Guid>
        where TEntity : AggregateRoot<Guid>
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

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entity
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TDestination>> GetAllAsync<TDestination>(Expression<Func<TEntity, TDestination>> selector)
        {
            return await _entity
                .Select(selector)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<TEntity?> FindAsync(TKey id)
        {
            return await _entity
                .Where(x => x.Id!.Equals(id))
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<TDestination?> FindAsync<TDestination>(TKey id, Expression<Func<TEntity, TDestination>> selector)
        {
            return await _entity
                .Where(x => x.Id!.Equals(id))
                .Select(selector)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
        }

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

        public  Task BulkAddAsync(IEnumerable<TEntity> entities, BulkConfig? bulkConfig = null)
            => _context.BulkInsertAsync(entities, bulkConfig);

        public Task BulkUpdateAsync(IEnumerable<TEntity> entities, BulkConfig? bulkConfig = null)
            =>  _context.BulkUpdateAsync(entities, bulkConfig);

        public Task BulkDeleteAsync(IEnumerable<TEntity> entities, BulkConfig? bulkConfig = null)
            => _context.BulkDeleteAsync(entities, bulkConfig);
    }
}
