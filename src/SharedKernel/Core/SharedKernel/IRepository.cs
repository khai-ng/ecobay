using Core.Aggregate;

namespace Core.SharedKernel
{
    public interface IRepository<TModel, TKey>
        where TModel : class, IAggregateRoot<TKey>
    {
    }

    //public interface IRepository<TModel> : IRepository<TModel, Ulid>
    //    where TModel : class, IAggregateRoot<Ulid>
    //{
    //}
}
