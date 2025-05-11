namespace Ordering.API.Application.IntegrationEvents
{
    public record OrderConfirmStockIntegrationEvent(
        Guid OrderId,
        IEnumerable<ProductQty> ProductQty) : IntegrationEvent
    { }

    public record ProductQty(string Id, int Qty)
    { }
}
