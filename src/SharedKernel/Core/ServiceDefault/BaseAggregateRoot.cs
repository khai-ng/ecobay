using Core.SharedKernel;

namespace Core.ServiceDefault
{
    public abstract class BaseAggregateRoot<TModel> : BaseAggregateRoot<TModel, Guid>
    {
        protected BaseAggregateRoot(Guid id) : base(id)
        {
        }
    }
}
