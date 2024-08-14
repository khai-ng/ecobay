using Core.SharedKernel;

namespace Core.EntityFramework.Repository
{
    public interface IQueryRepository<TModel> : IQueryRepository<TModel, Guid>
        where TModel : AggregateRoot<Guid>
    { }

    public interface IQueryRepository<TModel, TKey> : Core.Repository.IQueryRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
        IQueryable<TModel> Collection { get; }

    }
}
