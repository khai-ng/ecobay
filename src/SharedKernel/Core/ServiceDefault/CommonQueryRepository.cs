using Core.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Core.ServiceDefault
{
    public abstract class CommonQueryRepository<TModel> : CommonQueryRepository<TModel, Ulid>
        where TModel : BaseAggregateRoot<Ulid>
    {
        protected CommonQueryRepository(DbContext context) : base(context)
        {
        }
    }

    public abstract class CommonQueryRepository<TModel, TKey> : IRepository<TModel, TKey>
        where TModel : BaseAggregateRoot<TKey>
    {
        internal readonly DbContext _context;
        internal DbSet<TModel> _entity => _context.Set<TModel>();
        protected CommonQueryRepository(DbContext context)
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
