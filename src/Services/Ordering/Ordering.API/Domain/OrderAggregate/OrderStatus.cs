namespace Ordering.API.Domain.OrderAggregate
{
    public class OrderStatus(int id, string name) : Enumeration<OrderStatus>(id, name)
    {

        public static OrderStatus Submitted = new(1, nameof(Submitted).ToLowerInvariant());
        public static OrderStatus AwaitingValidation = new(2, nameof(AwaitingValidation).ToLowerInvariant());
        public static OrderStatus StockConfirmed = new(3, nameof(StockConfirmed).ToLowerInvariant());
        public static OrderStatus Paid = new(4, nameof(Paid).ToLowerInvariant());
        public static OrderStatus Shipped = new(5, nameof(Shipped).ToLowerInvariant());
        public static OrderStatus Cancelled = new(6, nameof(Cancelled).ToLowerInvariant());
    }
}
