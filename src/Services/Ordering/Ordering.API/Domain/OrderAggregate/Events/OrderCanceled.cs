namespace Ordering.API.Domain.OrderAggregate.Events
{
    public record OrderCanceled : DomainEvent
    {
        [JsonConstructor]
        private OrderCanceled() { }
        public OrderCanceled(Guid orderId) : base(orderId) { }
    }
}
