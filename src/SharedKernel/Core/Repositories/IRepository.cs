using Core.Entities;

namespace Core.Repositories
{
    public interface IRepository<TModel, TKey> :
        IQueryRepository<TModel, TKey>,
        ICommandRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    { }
}
