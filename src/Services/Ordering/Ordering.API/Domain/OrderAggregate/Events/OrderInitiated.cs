namespace Ordering.API.Domain.OrderAggregate.Events
{
    public class OrderInitiated : DomainEvent
    {
        public Order Order { get; set; }
        public DateTime TimeoutAt { get; private set; } = DateTime.UtcNow.AddDays(3);

        [JsonConstructor]
        private OrderInitiated() { }

        public OrderInitiated(Order order) : base(order.Id)
            => Order = order;

    }
}
