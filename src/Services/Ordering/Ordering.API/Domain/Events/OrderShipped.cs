using Core.EntityFramework.ServiceDefault;

namespace Ordering.API.Domain.Events
{
    public class OrderShipped : DomainEvent
    {
        public OrderShipped(Guid orderId) : base(orderId) { }
    }
}
