using MediatR;

namespace Core.IntegrationEvents
{
    public interface IDomainEvent<TKey> : INotification
    {
        TKey Id { get; }
        DateTime CreatedDate { get; }
    }
}
