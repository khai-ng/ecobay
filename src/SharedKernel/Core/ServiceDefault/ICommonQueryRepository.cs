using Core.SharedKernel;

namespace Core.ServiceDefault
{
    public interface ICommonQueryRepository<TModel> : ICommonQueryRepository<TModel, Ulid>
        where TModel : BaseAggregateRoot<Ulid>
    { }

    public interface ICommonQueryRepository<TModel, TKey>: IRepository<TModel, TKey>
        where TModel : BaseAggregateRoot<TKey>
    {
        IQueryable<TModel> GetQuery();
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel?> FindAsync(TKey id);
    }
}
