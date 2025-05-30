using Marten.Events.Aggregation;

namespace Ordering.API.Infrastruture.Projections
{
    public class OrderProjection : SingleStreamProjection<OrderView>
    {
        public OrderProjection() { }
        public void Apply(OrderInitiated @event, OrderView view)
        {
            view.Id = @event.Id;
            view.BuyerId = @event.BuyerId;
            view.PaymentId = @event.PaymentId;
            view.Address = @event.Address;
            view.TotalPrice = @event.TotalPrice;
            view.OrderItems = @event.OrderItems;
            view.CreatedAtTicks = @event.CreatedAt.Ticks;
            view.Status = OrderStatus.Submitted;
        }

        public void Apply(OrderStockConfirmed @event, OrderView view)   
            => view.Status = OrderStatus.StockConfirmed;

        public void Apply(OrderPaid @event, OrderView view)       
            => view.Status = OrderStatus.Paid;

        public void Apply(OrderShipped @event, OrderView view)
            => view.Status = OrderStatus.Shipped;

        public void Apply(OrderCanceled @event, OrderView view)
            => view.Status = OrderStatus.Cancelled;
    }
}
