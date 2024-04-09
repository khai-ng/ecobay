using MediatR;

namespace Core.SharedKernel
{
    public interface IEventHandler<TModel, TKey> : INotificationHandler<TModel>
        where TModel : IEvent<TKey>
    {
    }

    public interface IEventHandler<TModel> : IEventHandler<TModel, Ulid>
    where TModel : IEvent
    { }
}
