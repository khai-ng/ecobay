namespace Ordering.API.Domain.OrderAggregate.Events
{
    public record OrderPaid(Guid Id) : DomainEvent(Id)
    { }
}
