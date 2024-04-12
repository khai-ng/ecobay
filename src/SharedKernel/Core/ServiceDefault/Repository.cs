using Core.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Core.ServiceDefault
{
    public abstract class Repository<TModel> : Repository<TModel, Ulid>
        where TModel : AggregateRoot<Ulid>
    {
        protected Repository(DbContext context) : base(context)
        {
        }
    }

    public abstract class Repository<TModel, TKey> : 
        CommandRepository<TModel, TKey>,
        ICommonRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        protected Repository(DbContext context) : base(context)
        {
        }

        public IQueryable<TModel> GetQuery() => _entity;

        public async Task<IEnumerable<TModel>> GetAllAsync()
            => await _entity.ToListAsync();

        public async Task<TModel?> FindAsync(TKey id)
            => await _entity.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }
}
