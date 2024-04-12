using Core.SharedKernel;

namespace Core.ServiceDefault
{
    public interface ICommonRepository<TModel>: ICommonRepository<TModel, Ulid>
        where TModel : BaseAggregateRoot<Ulid>
    { }

    public interface ICommonRepository<TModel, TKey>: 
        ICommonQueryRepository<TModel, TKey>, 
        ICommonCommandRepository<TModel, TKey>
        where TModel : BaseAggregateRoot<TKey>
    { }
}
