using Core.Entities;
using Core.Repositories;

namespace Core.EntityFramework.Repositories
{
    public interface IRepository<TModel> : IRepository<TModel, Guid>
        where TModel : AggregateRoot<Guid>
    { }

    public interface IRepository<TModel, TKey> :
        IQueryRepository<TModel, TKey>,
        ICommandRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    { }
}
