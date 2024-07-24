using Core.EntityFramework.ServiceDefault;
using Ordering.API.Domain.OrderAggregate;
using System.ComponentModel.DataAnnotations;

namespace Ordering.API.Domain.OrderAgrregate
{
    public class Order : AggregateRoot
    {
        [MaxLength(26)]
        public string PaymentMethodId { get; private set; }
        [MaxLength(255)]
        public string Desciption { get; private set; } = string.Empty;
        public OrderStatus OrderStatus { get; private set; } = OrderStatus.Submitted;
        public Address Address { get; private set; }

        public IReadOnlyCollection<OrderItem> Items;
    }
}
