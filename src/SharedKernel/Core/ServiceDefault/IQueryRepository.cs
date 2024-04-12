using Core.SharedKernel;

namespace Core.ServiceDefault
{
    public interface IQueryRepository<TModel> : IQueryRepository<TModel, Ulid>
        where TModel : AggregateRoot<Ulid>
    { }

    public interface IQueryRepository<TModel, TKey>: IRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        IQueryable<TModel> GetQuery();
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel?> FindAsync(TKey id);
    }
}
