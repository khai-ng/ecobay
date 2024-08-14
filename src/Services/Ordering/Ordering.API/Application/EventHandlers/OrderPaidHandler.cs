using Core.EntityFramework.ServiceDefault;
using Core.Events.EventStore;
using Ordering.API.Domain.Events;
using Ordering.API.Domain.OrderAgrregate;

namespace Ordering.API.Application.EventHandlers
{
    public class OrderPaidHandler : IDomainEventHandler<OrderPaid>
    {
        private readonly IEventStoreRepository<Order> _eventStoreRepository;

        public OrderPaidHandler(IEventStoreRepository<Order> eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task Handle(OrderPaid @event, CancellationToken ct)
        {
            var order = await _eventStoreRepository.Find(@event.AggregateId, ct);
            ArgumentNullException.ThrowIfNull(order, nameof(order));

            order.SetPaid();
            await _eventStoreRepository.Update(@event.AggregateId, order, ct: ct);
        }
    }
}
