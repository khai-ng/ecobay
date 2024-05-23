using Core.SharedKernel;

namespace Core.Repository
{
    public interface IQueryRepository<TModel> : IQueryRepository<TModel, Ulid>
        where TModel : AggregateRoot<Ulid>
    { }

    public interface IQueryRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel?> FindAsync(TKey id);
    }
}
