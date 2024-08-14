using Core.EntityFramework.ServiceDefault;

namespace Ordering.API.Domain.Events
{
    public class OrderPaid : DomainEvent
    {
        public OrderPaid(Guid orderId) : base(orderId) { }
    }
}
