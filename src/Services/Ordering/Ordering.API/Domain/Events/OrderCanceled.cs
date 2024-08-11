using Core.EntityFramework.ServiceDefault;

namespace Ordering.API.Domain.Events
{
    public class OrderCanceled: DomainEvent
    {
        public Ulid OrderId { get; private set; }
        public OrderCanceled(Ulid orderId) : base(orderId)
            => OrderId = orderId; 
    }
}
