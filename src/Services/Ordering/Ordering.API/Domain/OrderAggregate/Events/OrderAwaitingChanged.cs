namespace Ordering.API.Domain.OrderAggregate.Events
{
    public record OrderAwaitingChanged(Guid Id) : DomainEvent(Id)
	{ }
}
