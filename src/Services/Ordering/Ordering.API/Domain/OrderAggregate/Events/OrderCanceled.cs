using Core.EntityFramework.ServiceDefault;
using System.Text.Json.Serialization;

namespace Ordering.API.Domain.OrderAggregate.Events
{
    public class OrderCanceled : DomainEvent
    {
        [JsonConstructor]
        private OrderCanceled() { }
        public OrderCanceled(Guid orderId) : base(orderId) { }
    }
}
