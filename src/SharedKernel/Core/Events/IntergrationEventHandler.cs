using MediatR;

namespace Core.Events
{
    public interface IIntergrationEventHandler<T> : INotificationHandler<T>
        where T : IntergrationEvent
    {
    }
}
