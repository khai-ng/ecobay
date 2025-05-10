namespace Ordering.API.Domain.OrderAggregate.Events
{
    public record OrderPaid : DomainEvent
    {
        [JsonConstructor]
        private OrderPaid() { }
        public OrderPaid(Guid orderId) : base(orderId) { }
    }
}
