namespace Ordering.API.Domain.OrderAggregate.Events
{
    public record OrderShipped(Guid Id) : DomainEvent(Id)
    { }
}
