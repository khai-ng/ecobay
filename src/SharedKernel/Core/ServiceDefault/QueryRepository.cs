using Core.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Core.ServiceDefault
{
    public abstract class QueryRepository<TModel> : QueryRepository<TModel, Ulid>
        where TModel : AggregateRoot<Ulid>
    {
        protected QueryRepository(DbContext context) : base(context)
        {
        }
    }

    public abstract class QueryRepository<TModel, TKey> : IRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        internal readonly DbContext _context;
        internal DbSet<TModel> _entity => _context.Set<TModel>();
        protected QueryRepository(DbContext context)
        {
            _context = context;
        }

        public IQueryable<TModel> GetQuery() => _entity;

        public async Task<IEnumerable<TModel>> GetAllAsync()
            => await _entity.ToListAsync();

        public async Task<TModel?> FindAsync(TKey id)
            => await _entity.SingleOrDefaultAsync(x => x.Id.Equals(id));      
    }
}
