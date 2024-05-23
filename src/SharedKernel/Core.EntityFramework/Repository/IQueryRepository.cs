using Core.SharedKernel;

namespace Core.EntityFramework.Repository
{
    public interface IQueryRepository<TModel> : IQueryRepository<TModel, Ulid>
        where TModel : AggregateRoot<Ulid>
    { }

    public interface IQueryRepository<TModel, TKey> : Core.Repository.IQueryRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        IQueryable<TModel> DbSet { get; }

    }
}
