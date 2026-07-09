using Ordering.API.Domain.OrderAggregate;

namespace Ordering.Test.Domain
{
    public class OrderAggregateTest
    {
        [Fact]
        public void create_order_success()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(
                Guid.CreateVersion7(), 
                Guid.CreateVersion7(), 
                address, 
                new List<OrderItem>());

            Assert.NotNull(order);
        }

        [Fact]
        public void valid_sum_total_price()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var items = new List<OrderItem>()
                {
                    new("123", "123", "image1.png", 10, 2),
                    new("124", "124", "image2.png", 15, 4),
                };
            var order = new Order(
                Guid.CreateVersion7(),
                Guid.CreateVersion7(),
                address,
                items);

            Assert.Equal(order.TotalPrice, items.Sum(x => x.Price * x.Qty));
        }

        [Fact]
        public void invalid_order_price_should_throw_exception()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");

            Assert.Throws<ArgumentOutOfRangeException>(
                () => new Order(
                    Guid.CreateVersion7(), 
                    Guid.CreateVersion7(), 
                    address,
                    new List<OrderItem>() { new("123", "123", "image.png", -1, 1) })
            );
        }

        [Fact]
        public void invalid_order_qty_should_throw_exception()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");

            Assert.Throws<ArgumentOutOfRangeException>(
                () => new Order(
                    Guid.CreateVersion7(),
                    Guid.CreateVersion7(),
                    address,
                    new List<OrderItem>() { new("123", "123", "image.png", 1, -1) })
            );
        }

        [Fact]
        public void validate_initial()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(Guid.CreateVersion7(), Guid.CreateVersion7(), address, new List<OrderItem>());

            Assert.Equal(1, order.Events.Count);

            order.SetCanceled();
            Assert.Equal(order.OrderStatus, OrderStatus.Cancelled);
            Assert.Equal(2, order.Events.Count);
        }

        [Fact]
        public void validate_stock_confirmed()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(
                Guid.CreateVersion7(),
                Guid.CreateVersion7(),
                address,
                new List<OrderItem>());

            order.SetStockConfirmed();

            Assert.Equal(order.OrderStatus, OrderStatus.StockConfirmed);
            Assert.Equal(2, order.Events.Count);

            order.SetCanceled();
            Assert.Equal(order.OrderStatus, OrderStatus.Cancelled);
            Assert.Equal(3, order.Events.Count);
        }

        [Fact]
        public void validate_paid()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(
                Guid.CreateVersion7(),
                Guid.CreateVersion7(),
                address,
                new List<OrderItem>());

            Assert.Throws<Exception>(() => order.SetPaid());

            order.SetStockConfirmed();
            order.SetPaid();

            Assert.Equal(order.OrderStatus, OrderStatus.Paid);
            Assert.Equal(3, order.Events.Count);

            order.SetCanceled();
            Assert.Equal(order.OrderStatus, OrderStatus.Cancelled);
            Assert.Equal(4, order.Events.Count);
        }

        [Fact]
        public void validate_shipped()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(
                Guid.CreateVersion7(),
                Guid.CreateVersion7(),
                address,
                new List<OrderItem>());

            Assert.Throws<Exception>(() => order.SetShipped());

            order.SetStockConfirmed();

            Assert.Throws<Exception>(() => order.SetShipped());

            order.SetPaid();
            order.SetShipped();

            Assert.Equal(order.OrderStatus, OrderStatus.Shipped);
            Assert.Equal(4, order.Events.Count);
            Assert.Throws<Exception>(() => order.SetCanceled());
        }
    }
}
