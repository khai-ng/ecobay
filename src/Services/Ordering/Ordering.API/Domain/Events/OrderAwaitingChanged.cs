using Core.EntityFramework.ServiceDefault;

namespace Ordering.API.Domain.Events
{
    public class OrderAwaitingChanged : DomainEvent
    {
        public OrderAwaitingChanged(Guid orderId) : base(orderId) { }

    }
}
