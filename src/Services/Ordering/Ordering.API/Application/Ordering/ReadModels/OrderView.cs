namespace Ordering.API.Application.Ordering.ReadModels
{
    public class OrderView
    {
        public Guid Id { get; set; }
        public Guid BuyerId { get; set; }
        public Guid PaymentId { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public long CreatedAtTicks { get; set; }
        public Address Address { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}