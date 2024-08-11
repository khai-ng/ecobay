using Core.EntityFramework.ServiceDefault;

namespace Ordering.API.Domain.Events
{
    public class OrderPaid : DomainEvent
    {
        public Ulid OrderId { get; private set; }
        public OrderPaid(Ulid orderId) : base(orderId)
            => OrderId = orderId;
    }
}
