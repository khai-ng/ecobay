using Core.Autofac;
using Core.IntegrationEvents.IntegrationEvents;
using MediatR;
using ProductAggregate.API.Application.Product.Update;

namespace ProductAggregate.API.Application.IntegrationEvents
{
    
    public class OrderConfirmStockIntegrationEventHandler :
        IIntegrationEventHandler<OrderConfirmStockIntegrationEvent>, ITransient
    {
        private readonly IMediator _mediator;

        public OrderConfirmStockIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task HandleAsync(OrderConfirmStockIntegrationEvent @event, CancellationToken ct = default)
        {
            var request = new ConfimStockRequest(@event.ProductUnits);

            await _mediator.Send(request, ct);
        }
    }
}
