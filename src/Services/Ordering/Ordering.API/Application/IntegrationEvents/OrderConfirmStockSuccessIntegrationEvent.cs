namespace Ordering.API.Application.IntegrationEvents
{
    public class OrderConfirmStockSuccessIntegrationEvent(Guid orderId) : IntegrationEvent
    {
        public Guid OrderId { get; set; } = orderId;
    }
}
