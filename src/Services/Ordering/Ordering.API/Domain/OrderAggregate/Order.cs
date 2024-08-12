using Core.EntityFramework.ServiceDefault;
using Ordering.API.Domain.Events;
using Ordering.API.Domain.OrderAggregate;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.API.Domain.OrderAgrregate
{
    public class Order : AggregateRoot
    {
        public Ulid BuyerId { get; private set; }
        [MaxLength(26)]
        public string PaymentId { get; private set; }
        [ForeignKey(nameof(OrderStatus))]
        public int OrderStatusId { get; private set; }
        [MaxLength(255)]
        public string? Desciption { get; private set; } = string.Empty;
        [Column(TypeName = "decimal(12,2)")]
        public decimal TotoalPrice { get; private set; } = 0;
        
        public Address Address { get; private set; }
        
        public DateTimeOffset? CreatedDate { get; private set; }

        public List<OrderItem> OrderItems {  get; private set; }

        public OrderStatus OrderStatus { get; private set; }
        protected Order() 
        {
            OrderItems = [];
        }
        public Order(Ulid buyerId, string paymentId, Address address, IEnumerable<OrderItem> orderItems) 
        {
            Id = Ulid.NewUlid();
            BuyerId = buyerId;
            PaymentId = paymentId;
            OrderStatusId = OrderStatus.Submitted.Id;
            Address = address;
            CreatedDate = DateTimeOffset.Now;
            OrderItems = orderItems.ToList();
            TotoalPrice = OrderItems.Sum(x => x.UnitPrice * x.Unit);
            AddOrderEvent();
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            var exist = OrderItems.FirstOrDefault(x => x.ProductId == orderItem.ProductId);
            if (exist != null)
                exist.AddUnits(orderItem.Unit);
            else
                OrderItems.Add(orderItem);

            TotoalPrice += orderItem.UnitPrice * orderItem.Unit;
        }
       
        public void SetPaid()
        {
            PaidOrderEvent();
        }

        public void SetShipped()
        {
            if (OrderStatusId != OrderStatus.Paid.Id)
                throw new Exception($"Can not change status from {OrderStatus.Name} to {OrderStatus.Paid.Name}");

            ShippedOrderEvent();
        }

        public void SetCanceled()
        {
            if (OrderStatusId == OrderStatus.Paid.Id || OrderStatusId == OrderStatus.Shipped.Id)
                throw new Exception($"Can not change status from {OrderStatus.Name} to {OrderStatus.Shipped.Name}");

            OrderStatusId = OrderStatus.Cancelled.Id;

            CancelOrderEvent();
        }

        //Events
        private void AddOrderEvent()
        {
            var addOrderEvent = new OrderInitiated(Id, BuyerId, OrderItems);
            Enqueue(addOrderEvent);
        }

        private void PaidOrderEvent()
        {
            var paidOrderEvent = new OrderPaid(Id);
            Enqueue(paidOrderEvent);
        }

        private void ShippedOrderEvent()
        {
            var paidOrderEvent = new OrderShipped(Id);
            Enqueue(paidOrderEvent);
        }

        private void CancelOrderEvent()
        {
            var cancelOrderEvent = new OrderCanceled(Id);
            Enqueue(cancelOrderEvent);
        }
    }
}
