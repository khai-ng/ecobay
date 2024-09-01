using Core.IntegrationEvents.IntegrationEvents;
using ProductAggregate.API.Application.Dto.Product;

namespace ProductAggregate.API.Application.IntegrationEvents
{
    public class OrderConfirmStockIntegrationEvent(
        Guid orderId,
        IEnumerable<ProductUnit> productUnits) : IntegrationEvent
    {
        public Guid OrderId { get; } = orderId;
        public IEnumerable<ProductUnit> ProductUnits { get; } = productUnits;
    }
}
