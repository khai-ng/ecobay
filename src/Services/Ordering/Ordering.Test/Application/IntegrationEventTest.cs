using Core.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Ordering.API.Application.IntegrationEvents;
using Ordering.API.Domain.OrderAggregate;

namespace Ordering.Test.Application
{
    public class IntegrationEventTest
    {
        [Fact]
        public async ValueTask OrderConfirmStockSuccess_ShouldUpdateOrderStatusToStockConfirmedAsync()
        {
            var address = new Address("vn", "hcm", "d1", "2/1");
            var items = new List<OrderItem>()
                {
                    new("123", "123", "image1.png", 10, 2),
                };
            var order = new Order(
                Guid.CreateVersion7(),
                Guid.CreateVersion7(),
                address,
                items);

            var repositoryMock = new Mock<IEventStoreRepository<Order>>();
            repositoryMock.Setup(x => x.FindAsync(
                It.IsAny<Guid>(), 
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(order);

            repositoryMock.Setup(x => x.UpdateAsync(
                It.IsAny<Guid>(), 
                It.IsAny<Order>(), 
                It.IsAny<long>(), 
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(1);

            var loggerMock = new Mock<ILogger<OrderConfirmStockSuccessIntegrationEventHandler>>();

            loggerMock.Setup(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ));

            var evt = new OrderConfirmStockSuccessIntegrationEvent(order.Id);
            var handler = new OrderConfirmStockSuccessIntegrationEventHandler(repositoryMock.Object, loggerMock.Object);
            await handler.HandleAsync(evt, TestContext.Current.CancellationToken);

            Assert.Equal(order.OrderStatus, OrderStatus.StockConfirmed);
        }
    }
}
