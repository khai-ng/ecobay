using Core.Repositories;
using Moq;
using Ordering.API.Application.Abstractions;
using Ordering.API.Application.Ordering;
using Ordering.API.Application.Ordering.ReadModels;
using Ordering.API.Domain.OrderAggregate;

namespace Ordering.Test.Application
{
    public class OrderingTest
    {
        [Fact]
        public async ValueTask get_success()
        {
            var data = new List<OrderView>()
            {
                new() { Id = Guid.NewGuid(), BuyerId = Guid.NewGuid(), Status = OrderStatus.StockConfirmed },
                new() { Id = Guid.NewGuid(), BuyerId = Guid.NewGuid(), Status = OrderStatus.StockConfirmed },
            };

            var repositoryMock = new Mock<IOrderRepository>();
            repositoryMock.Setup(x => x.GetAsync(
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<int?>()
            )).ReturnsAsync(data);

            var request = new GetOrderCommand();
            var handler = new GetOrder(repositoryMock.Object);

            var result = await handler.Handle(request);
            Assert.True(result.IsSuccess);
            for (var i = 0; i < data.Count; i++)
            {
                Assert.Equal(data[i].Id, result.Data?.ElementAt(i).Id);
            }
        }

        [Fact]
        public async ValueTask create_order_success()
        {
            var orderItems = new List<OrderItemCommand>{
                new("1", "test1", "iamge1.png", 1, 1),
                new("2", "test2", "iamge2.png", 2, 2),
            };
            var request = new CreateOrderCommand(Guid.NewGuid(), Guid.NewGuid(), "country", "city", "district", "street", orderItems);

            var repositoryMock = new Mock<IEventStoreRepository<Order>>();
            repositoryMock.Setup(x => x.AddAsync(
                It.IsAny<Guid>(),
                It.IsAny<Order>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(1);

            var userMock = new Mock<IUser>();
            userMock.Setup(x => x.Info())
                .Returns(new API.Infrastruture.Services.UserInfo()
                {
                    Id = Guid.NewGuid(),
                    Name = "test",
                    Email = "test@gmail.com",
                });

            var handler = new CreateOrder(repositoryMock.Object, userMock.Object);
            var result = await handler.Handle(request, TestContext.Current.CancellationToken);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void invalid_qty_should_throw_exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new List<OrderItemCommand>{
                new("1", "test1", "iamge1.png", 1, 1),
                new("2", "test2", "iamge2.png", 2, -2),
            });
        }

        [Fact]
        public void invalid_price_should_throw_exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new List<OrderItemCommand>{
                new("1", "test1", "iamge1.png", 1, 1),
                new("2", "test2", "iamge2.png", -2, 2),
            });
        }
    }
}
