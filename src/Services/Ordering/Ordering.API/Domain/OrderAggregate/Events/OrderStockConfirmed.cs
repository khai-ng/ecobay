using Core.EntityFramework.ServiceDefault;
using System.Text.Json.Serialization;

namespace Ordering.API.Domain.OrderAggregate.Events
{
    public class OrderStockConfirmed : DomainEvent
    {
        [JsonConstructor]
        private OrderStockConfirmed() { }
        public OrderStockConfirmed(Guid orderId) : base(orderId) { }

    }
}
