namespace Ordering.API.Domain.OrderAggregate.Events
{
    public record OrderInitiated(
		Guid Id,
		Guid BuyerId,
		Guid PaymentId,
		Address Address,
		List<OrderItem> OrderItems,
		decimal TotalPrice
		) : DomainEvent(Id)
    { }
}
