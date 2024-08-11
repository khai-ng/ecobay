using Core.EntityFramework.ServiceDefault;
using Ordering.API.Domain.Events;

namespace Ordering.API.Application.EventHandlers
{
    public class OrderInitiatedHandler : IDomainEventHandler<OrderInitiated>
    {
        public Task Handle(OrderInitiated notification, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
