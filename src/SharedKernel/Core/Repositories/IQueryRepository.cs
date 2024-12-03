using Core.Entities;

namespace Core.Repositories
{
    public interface IQueryRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel?> FindAsync(TKey id);
    }
}
