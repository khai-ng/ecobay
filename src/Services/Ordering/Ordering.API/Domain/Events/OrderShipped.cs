using Core.EntityFramework.ServiceDefault;

namespace Ordering.API.Domain.Events
{
    public class OrderShipped : DomainEvent
    {
        public Ulid OrderId { get; private set; }
        public OrderShipped(Ulid orderId) : base(orderId)
            => OrderId = orderId; 
    }
}
