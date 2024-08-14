using Core.EntityFramework.ServiceDefault;

namespace Ordering.API.Domain.Events
{
    public class OrderCanceled: DomainEvent
    {
        public OrderCanceled(Guid orderId) : base(orderId) { }
    }
}
