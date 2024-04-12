using Core.Aggregate;

namespace Core.ServiceDefault
{
    public interface ICommonRepository<TModel>: ICommonRepository<TModel, Ulid>
        where TModel : class, IAggregateRoot<Ulid>
    { }

    public interface ICommonRepository<TModel, TKey>: 
        ICommonQueryRepository<TModel, TKey>, 
        ICommonCommandRepository<TModel, TKey>
        where TModel : class, IAggregateRoot<TKey>
    { }
}
