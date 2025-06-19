namespace Ordering.API.Application.Ordering
{
    public record CreateOrderCommand(
        Guid? BuyerId,
        Guid PaymentId,
        string Country,
        string City,
        string District,
        string Street,
        List<OrderItemCommand> OrderItems) : IRequest<AppResult<Guid>>
    { }

    public record OrderItemCommand(
        string ProductId,
        string ProductName,
        string ImageUrl,
        decimal Price,
        int Qty)
    {
        public int Qty { get; } = Qty > 0 
            ? Qty 
            : throw new ArgumentOutOfRangeException("Argument can not nigative", nameof(Qty));
    }
}
