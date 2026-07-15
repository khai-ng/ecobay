using Ordering.API.Domain.OrderAggregate;

namespace Ordering.Test.Domain
{
    public class OrderAggregateTest
    {
        [Fact]
        public void CreateOrder_ShouldCreatesInstance()
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
        public void TotalPrice_ShouldEqualsSumOfOrderItemPrices()
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
        public void OrderInitiated_WithNegativePrice_ShouldThrowsArgumentOutOfRangeException()
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
        public void Order_WhenInitiated_WithNegativeQuantity_ShouldThrowsArgumentOutOfRangeException()
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
        public void Order_WhenSetCanceled_ShouldUpdatesStatusToCancelledAndEnqueuesEvent()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(Guid.CreateVersion7(), Guid.CreateVersion7(), address, new List<OrderItem>());

            Assert.Single(order.Events);

            order.SetCanceled();
            Assert.Equal(order.OrderStatus, OrderStatus.Cancelled);
            Assert.Equal(2, order.Events.Count);
        }

        [Fact]
        public void Order_WhenSetStockConfirmed_UpdatesStatusToStockConfirmedAndEnqueuesEvent()
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
        public void Order_WhenInitiated_ThenSetsStatusToPaid_ShouldThrowsException()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(
                Guid.CreateVersion7(),
                Guid.CreateVersion7(),
                address,
                new List<OrderItem>());

            Assert.Throws<Exception>(() => order.SetPaid());
        }

        [Fact]
        public void SetPaid_RequiresStockConfirmed_ThenSetsStatusToPaidAndEnqueuesEvent()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(
                Guid.CreateVersion7(),
                Guid.CreateVersion7(),
                address,
                new List<OrderItem>());

            order.SetStockConfirmed();
            order.SetPaid();

            Assert.Equal(order.OrderStatus, OrderStatus.Paid);
            Assert.Equal(3, order.Events.Count);

            order.SetCanceled();
            Assert.Equal(order.OrderStatus, OrderStatus.Cancelled);
            Assert.Equal(4, order.Events.Count);
        }

        [Fact]
        public void SetShipped_RequiresPaid_ThenSetsStatusToShippedAndEnqueuesEvent()
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
