using Core.EntityFramework.ServiceDefault;

namespace Ordering.API.Domain.Events
{
    public class OrderAwaitingChanged : DomainEvent
    {
        public Ulid OrderId { get; private set; }
        public OrderAwaitingChanged(Ulid orderId) : base(orderId)
            => OrderId = orderId;

    }
}
