namespace Ordering.API.Application.Services
{
    public record CreateOrderCommand(
        Guid BuyerId,
        Guid PaymentId,
        string Country,
        string City,
        string District,
        string Street,
        List<OrderItemCommand> OrderItems) : IRequest<AppResult<Guid>>
    { }

    public record OrderItemCommand(
        string ProductId,
        decimal Price,
        int Qty)
    {
        public int Qty { get; } = Qty > 0 
            ? Qty 
            : throw new ArgumentOutOfRangeException("Argument can not nigative", nameof(Qty));
    }
}
