namespace Core.SharedKernel
{
    public interface IRepository<TModel, TKey>
        where TModel : AggregateRoot<TKey>
    {
    }
}
