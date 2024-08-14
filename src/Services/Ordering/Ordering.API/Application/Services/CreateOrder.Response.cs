namespace Ordering.API.Application.Services
{
    public class CreateOrderResponse(Guid orderId)
    {
        public Guid OrderId { get; set; } = orderId;
    }
}
