using Core.Aggregate;
using Core.SharedKernel;

namespace Core.ServiceDefault
{
    public interface ICommonQueryRepository<TModel> : ICommonQueryRepository<TModel, Ulid>
        where TModel : class, IAggregateRoot<Ulid>
    { }

    public interface ICommonQueryRepository<TModel, TKey>: IRepository<TModel, TKey>
        where TModel : class, IAggregateRoot<TKey>
    {
        IQueryable<TModel> GetQuery();
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel?> FindAsync(TKey id);
    }
}
