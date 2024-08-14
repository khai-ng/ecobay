using Core.EntityFramework.ServiceDefault;
using Ordering.API.Domain.OrderAgrregate;

namespace Ordering.API.Domain.Events
{
    public class OrderInitiated : DomainEvent
    {
        public Order Order { get; set; }
        public DateTime TimeoutAt { get; private set; } = DateTime.UtcNow.AddDays(3);

        public OrderInitiated(Order order) : base(order.Id)     
            => Order = order;
        
    }
}
