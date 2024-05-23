using Core.SharedKernel;

namespace Core.Repository
{
    public interface IRepository<TModel, TKey> :
        IQueryRepository<TModel, TKey>,
        ICommandRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    { }
}
