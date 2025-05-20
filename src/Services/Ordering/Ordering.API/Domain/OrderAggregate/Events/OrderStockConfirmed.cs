namespace Ordering.API.Domain.OrderAggregate.Events
{
    public record OrderStockConfirmed(Guid Id) : DomainEvent(Id)
	{ }
}
