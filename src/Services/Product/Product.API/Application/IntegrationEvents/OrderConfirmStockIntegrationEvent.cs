namespace Product.API.Application.IntegrationEvents
{
    public class OrderConfirmStockIntegrationEvent(
        Guid orderId,
        IEnumerable<ProductQtyDto> productQty) : IntegrationEvent
    {
        public Guid OrderId { get; } = orderId;
        public IEnumerable<ProductQtyDto> ProductQty { get; } = productQty;
    }
}
