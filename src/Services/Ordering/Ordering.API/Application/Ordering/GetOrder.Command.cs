namespace Ordering.API.Application.Ordering
{
	public record GetOrderCommand(Guid? Id = null, Guid? BuyerId = null, int? OrderStatusId = null) : IRequest<AppResult<IEnumerable<OrderView>>>
	{ }
}
