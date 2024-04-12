using Core.SharedKernel;

namespace Core.ServiceDefault
{
    public interface IRepository<TModel>: ICommonRepository<TModel, Ulid>
        where TModel : AggregateRoot<Ulid>
    { }

    public interface ICommonRepository<TModel, TKey>: 
        IQueryRepository<TModel, TKey>, 
        ICommandRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    { }
}
