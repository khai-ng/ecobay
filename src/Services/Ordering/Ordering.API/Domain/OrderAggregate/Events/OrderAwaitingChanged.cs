namespace Ordering.API.Domain.OrderAggregate.Events
{
    public record OrderAwaitingChanged : DomainEvent
    {
        [JsonConstructor]
        private OrderAwaitingChanged() { }
        public OrderAwaitingChanged(Guid orderId) : base(orderId) { }

    }
}
