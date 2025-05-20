namespace Ordering.API.Domain.OrderAggregate.Events
{
    public record OrderCanceled(Guid Id) : DomainEvent(Id)
	{ }
}
