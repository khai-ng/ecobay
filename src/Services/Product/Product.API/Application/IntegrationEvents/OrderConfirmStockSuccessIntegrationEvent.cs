namespace Product.API.Application.IntegrationEvents
{
    public record OrderConfirmStockSuccessIntegrationEvent(Guid OrderId) : IntegrationEvent
    { }
}
