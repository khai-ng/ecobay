using Core.Aggregate;
using Core.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Core.ServiceDefault
{
    public abstract class CommonRepository<TModel> : CommonRepository<TModel, Ulid>
        where TModel : class, IAggregateRoot<Ulid>
    {
        protected CommonRepository(DbContext context) : base(context)
        {
        }
    }

    public abstract class CommonRepository<TModel, TKey> : 
        CommonCommandRepository<TModel, TKey>,
        ICommonRepository<TModel, TKey>
        where TModel : class, IAggregateRoot<TKey>
    {
        protected CommonRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<TModel> GetQuery() => _entity;

        public async Task<IEnumerable<TModel>> GetAllAsync()
            => await _entity.ToListAsync();

        public async Task<TModel?> FindAsync(TKey id)
            => await _entity.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }
}
