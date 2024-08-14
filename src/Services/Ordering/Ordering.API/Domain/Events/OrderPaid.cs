using Core.EntityFramework.ServiceDefault;
using System.Text.Json.Serialization;

namespace Ordering.API.Domain.Events
{
    public class OrderPaid : DomainEvent
    {
        [JsonConstructor]
        private OrderPaid() { }  
        public OrderPaid(Guid orderId) : base(orderId) { }
    }
}
