using Core.EntityFramework.ServiceDefault;
using Core.Events.DomainEvents;
using Ordering.API.Domain.Events;
using Ordering.API.Domain.OrderAggregate;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Ordering.API.Domain.OrderAgrregate
{
    public class Order : AggregateRoot
    {
        public Guid BuyerId { get; private set; }
        public Guid PaymentId { get; private set; }
        [ForeignKey(nameof(OrderStatus))]
        public int OrderStatusId { get; private set; }
        [MaxLength(255)]
        public string? Description { get; private set; } = string.Empty;
        [Column(TypeName = "decimal(12,2)")]
        public decimal TotalPrice { get; private set; } = 0;
        
        public Address Address { get; private set; }
        
        public DateTime? CreatedDate { get; private set; }

        public List<OrderItem> OrderItems { get; private set; } = [];

        public OrderStatus OrderStatus { get; private set; }

        [JsonConstructor]
        private Order() { }
        public Order(Guid buyerId, Guid paymentId, Address address, IEnumerable<OrderItem> orderItems)
        {
            Id = Guid.NewGuid();
            BuyerId = buyerId;
            PaymentId = paymentId;
            OrderStatusId = OrderStatus.Submitted.Id;
            Address = address;
            CreatedDate = DateTime.UtcNow;
            OrderItems = orderItems.ToList();
            TotalPrice = OrderItems.Sum(x => x.UnitPrice * x.Unit);

            Enqueue(new OrderInitiated(this));
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            var exist = OrderItems.FirstOrDefault(x => x.ProductId == orderItem.ProductId);
            if (exist != null)
                exist.AddUnits(orderItem.Unit);
            else
                OrderItems.Add(orderItem);

            TotalPrice += orderItem.UnitPrice * orderItem.Unit;
        }

        public void SetStockConfirmed()
        {
            OrderStatusId = OrderStatus.StockConfirmed.Id;
            Enqueue(new OrderStockConfirmed(Id));
        }

        public void SetPaid()
        {
            OrderStatusId = OrderStatus.Paid.Id;
            Enqueue(new OrderPaid(Id));
        }

        public void SetShipped()
        {
            if (OrderStatusId != OrderStatus.Paid.Id)
                throw new Exception($"Can not change status from {OrderStatus.Name} to {OrderStatus.Paid.Name}");

            OrderStatusId = OrderStatus.Shipped.Id;
            Enqueue(new OrderShipped(Id));
        }

        public void SetCanceled()
        {
            if (OrderStatusId == OrderStatus.Paid.Id || OrderStatusId == OrderStatus.Shipped.Id)
                throw new Exception($"Can not change status from {OrderStatus.Name} to {OrderStatus.Shipped.Name}");

            OrderStatusId = OrderStatus.Cancelled.Id;
            Enqueue(new OrderCanceled(Id));
        }
        public override void Apply(IDomainEvent<Guid> @event)
        {
            switch (@event)
            {
                case OrderInitiated orderInitiated:
                    Id = orderInitiated.Order.Id;
                    BuyerId = orderInitiated.Order.BuyerId;
                    PaymentId = orderInitiated.Order.PaymentId;
                    OrderStatusId = OrderStatus.Submitted.Id;
                    Description = orderInitiated.Order.Description;
                    TotalPrice = orderInitiated.Order.TotalPrice;
                    Address = orderInitiated.Order.Address;
                    CreatedDate = orderInitiated.Order.CreatedDate;
                    OrderItems = orderInitiated.Order.OrderItems;
                    break;
                case OrderStockConfirmed _:
                    OrderStatusId = OrderStatus.StockConfirmed.Id;
                    break;
                case OrderPaid _:
                    OrderStatusId = OrderStatus.Paid.Id;
                    break; 
                case OrderShipped _:
                    OrderStatusId = OrderStatus.Shipped.Id;
                    break; 
                case OrderCanceled _:
                    OrderStatusId = OrderStatus.Cancelled.Id;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(@event));
            }
        }
    }
}
