namespace Product.API.Application.IntegrationEvents
{
    public record OrderConfirmStockIntegrationEvent(
        Guid OrderId,
        IEnumerable<ProductQtyDto> ProductQty) : IntegrationEvent
    { }
}
