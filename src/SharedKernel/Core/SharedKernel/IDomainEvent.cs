using MediatR;

namespace Core.SharedKernel
{
    public interface IDomainEvent<TKey> : INotification
    {
        TKey Id { get; }
       DateTime TimeStamp { get; }
    }
}
