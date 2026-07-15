using Ordering.API.Domain.OrderAggregate;
using Ordering.API.Domain.OrderAggregate.Events;

namespace Ordering.Test.Domain
{
    public class OrderAggregateTests
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
        public void CreateOrder_ShouldInitializeWithSubmittedStatusAndCorrectProperties()
        {
            var buyerId = Guid.CreateVersion7();
            var paymentId = Guid.CreateVersion7();
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(buyerId, paymentId, address, new List<OrderItem>());

            Assert.Equal(OrderStatus.Submitted, order.OrderStatus);
            Assert.Equal(buyerId, order.BuyerId);
            Assert.Equal(paymentId, order.PaymentId);
            Assert.Equal(address, order.Address);
            Assert.Single(order.Events);
            Assert.IsType<OrderInitiated>(order.Events.First());
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
        public void AddOrderItem_WithNewProduct_ShouldAddItemAndUpdateTotalPrice()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(Guid.CreateVersion7(), Guid.CreateVersion7(), address, new List<OrderItem>());

            var item = new OrderItem("p1", "Product 1", "image1.png", 10, 3);
            order.AddOrderItem(item);

            Assert.Single(order.OrderItems);
            Assert.Equal(30, order.TotalPrice);
        }

        [Fact]
        public void AddOrderItem_WithExistingProduct_ShouldMergeQtyAndUpdateTotalPrice()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var items = new List<OrderItem>() { new("p1", "Product 1", "image1.png", 10, 2) };
            var order = new Order(Guid.CreateVersion7(), Guid.CreateVersion7(), address, items);

            order.AddOrderItem(new OrderItem("p1", "Product 1", "image1.png", 10, 3));

            Assert.Single(order.OrderItems);
            Assert.Equal(5, order.OrderItems[0].Qty);
            Assert.Equal(50, order.TotalPrice);
        }

        [Fact]
        public void OrderItem_AddQty_ShouldIncreaseQuantity()
        {
            var item = new OrderItem("p1", "Product 1", "image1.png", 10, 2);
            item.AddQty(5);
            Assert.Equal(7, item.Qty);
        }

        [Fact]
        public void SetStockConfirmed_WhenAlreadyConfirmed_ShouldThrowException()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(Guid.CreateVersion7(), Guid.CreateVersion7(), address, new List<OrderItem>());
            order.SetStockConfirmed();

            Assert.Throws<Exception>(() => order.SetStockConfirmed());
        }

        [Fact]
        public void SetPaid_WhenAlreadyPaid_ShouldThrowException()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(Guid.CreateVersion7(), Guid.CreateVersion7(), address, new List<OrderItem>());
            order.SetStockConfirmed();
            order.SetPaid();

            Assert.Throws<Exception>(() => order.SetPaid());
        }

        [Fact]
        public void Address_WithSameValues_ShouldBeEqual()
        {
            var a1 = new Address("vn", "hcm", "d1", "2/1");
            var a2 = new Address("vn", "hcm", "d1", "2/1");
            Assert.Equal(a1, a2);
        }

        [Fact]
        public void Address_WithDifferentValues_ShouldNotBeEqual()
        {
            var a1 = new Address("vn", "hcm", "d1", "2/1");
            var a2 = new Address("vn", "hcm", "d1", "3/1");
            Assert.NotEqual(a1, a2);
        }

        [Fact]
        public void Order_WhenSetPaid_ShouldUpdateStatusToPaidAndEnqueuesEvent()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(Guid.CreateVersion7(), Guid.CreateVersion7(), address, new List<OrderItem>());
            order.SetStockConfirmed();

            order.SetPaid();

            Assert.Equal(OrderStatus.Paid, order.OrderStatus);
            Assert.Equal(3, order.Events.Count);
        }

        [Fact]
        public void Order_WhenSetShipped_ShouldUpdateStatusToShippedAndEnqueuesEvent()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(Guid.CreateVersion7(), Guid.CreateVersion7(), address, new List<OrderItem>());
            order.SetStockConfirmed();
            order.SetPaid();

            order.SetShipped();

            Assert.Equal(OrderStatus.Shipped, order.OrderStatus);
            Assert.Equal(4, order.Events.Count);
        }

        [Fact]
        public void Order_WhenSetShipped_WhenNotPaid_ShouldThrowException()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(Guid.CreateVersion7(), Guid.CreateVersion7(), address, new List<OrderItem>());

            Assert.Throws<Exception>(() => order.SetShipped());
        }

        [Fact]
        public void Order_WhenSetCanceled_WhenShipped_ShouldThrowException()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(Guid.CreateVersion7(), Guid.CreateVersion7(), address, new List<OrderItem>());
            order.SetStockConfirmed();
            order.SetPaid();
            order.SetShipped();

            Assert.Throws<Exception>(() => order.SetCanceled());
        }

        [Fact]
        public void Order_WhenSetCanceled_WhenPaid_ShouldUpdateStatusToCancelledAndEnqueuesEvent()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var order = new Order(Guid.CreateVersion7(), Guid.CreateVersion7(), address, new List<OrderItem>());
            order.SetStockConfirmed();
            order.SetPaid();

            order.SetCanceled();

            Assert.Equal(OrderStatus.Cancelled, order.OrderStatus);
            Assert.Equal(4, order.Events.Count);
        }

        [Fact]
        public void OrderItem_WithZeroPrice_ShouldNotThrow()
        {
            var item = new OrderItem("p1", "Product 1", "image1.png", 0, 1);
            Assert.Equal(0, item.Price);
        }

        [Fact]
        public void OrderItem_WithZeroQty_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new OrderItem("p1", "Product 1", "image1.png", 10, 0));
        }
    }
}
