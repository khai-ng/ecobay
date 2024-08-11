using Core.EntityFramework.ServiceDefault;
using Ordering.API.Domain.OrderAgrregate;

namespace Ordering.API.Domain.Events
{
    public class OrderInitiated : DomainEvent
    {
        public Ulid OrderId { get; private set; }
        public Ulid BuyerId { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems { get; private set; }
        public decimal TotalPrice { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;
        public DateTimeOffset TimeoutAt { get; private set; } = DateTimeOffset.Now.AddDays(3);

        public OrderInitiated(Ulid orderId,
            Ulid buyerId,
            IReadOnlyCollection<OrderItem> orderItems) : base(orderId)
        {
            if (orderItems == null || orderItems.Count == 0) throw new ArgumentException(nameof(orderItems));

            OrderId = orderId;
            BuyerId = buyerId;
            OrderItems = orderItems;
            TotalPrice = orderItems.Sum(x => x.UnitPrice * x.Unit);
        }
    }
}
