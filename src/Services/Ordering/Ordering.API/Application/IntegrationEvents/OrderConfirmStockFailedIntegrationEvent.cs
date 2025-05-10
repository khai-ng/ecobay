namespace Ordering.API.Application.IntegrationEvents
{
    public record OrderConfirmStockFailedIntegrationEvent(Guid OrderId, string Reason) : IntegrationEvent
    { }
}
