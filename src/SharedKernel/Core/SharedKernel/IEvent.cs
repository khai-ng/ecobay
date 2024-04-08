using MediatR;

namespace Core.SharedKernel
{
    public interface IEvent<out TKey> : INotification
    {
        TKey Id { get; }
        long Version { get; }

    }

    public interface IEvent : IEvent<Guid> { }
}
