namespace Ordering.API.Domain.OrderAggregate.Events
{
    public record OrderShipped : DomainEvent
    {
        [JsonConstructor]
        private OrderShipped() { }
        public OrderShipped(Guid orderId) : base(orderId) { }
    }
}
