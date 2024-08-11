using Core.EntityFramework.ServiceDefault;
using Core.Events.EventStore;
using Ordering.API.Domain.Events;
using Ordering.API.Domain.OrderAgrregate;

namespace Ordering.API.Application.EventHandlers
{
    public class OrderInitiatedHandler : IDomainEventHandler<OrderInitiated>
    {
        //private readonly IEventStoreRepository<Order> _eventStoreRepository;

        //public OrderInitiatedHandler(IEventStoreRepository<Order> eventStoreRepository)
        //{
        //    _eventStoreRepository = eventStoreRepository;
        //}
        public OrderInitiatedHandler() { }

        public Task Handle(OrderInitiated @event, CancellationToken ct)
        {
            //_eventStoreRepository.Add(@event.OrderId);
            return Task.CompletedTask;
        }
    }
}
