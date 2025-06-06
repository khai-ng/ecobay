namespace Ordering.API.Presentation.Endpoints
{
    public class CreateOrderRequest(
        Guid PaymentId,
        string Country,
        string City,
        string District,
        string Street,
        List<OrderItemCommand> OrderItems)
    { }
}
