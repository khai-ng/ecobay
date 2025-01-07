namespace Product.API.Application.IntegrationEvents
{
    public class OrderConfirmStockIntegrationEvent(
        Guid orderId,
        IEnumerable<ProductUnitDto> productUnits) : IntegrationEvent
    {
        public Guid OrderId { get; } = orderId;
        public IEnumerable<ProductUnitDto> ProductUnits { get; } = productUnits;
    }
}
