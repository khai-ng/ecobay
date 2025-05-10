namespace Ordering.API.Domain.OrderAggregate.Events
{
    public record OrderStockConfirmed : DomainEvent
    {
        [JsonConstructor]
        private OrderStockConfirmed() { }
        public OrderStockConfirmed(Guid orderId) : base(orderId) { }

    }
}
