using Core.IntegrationEvents.IntegrationEvents;
using Moq;
using Ordering.API.Application.Abstractions;
using Ordering.API.Application.DomainEventHandlers;
using Ordering.API.Application.IntegrationEvents;
using Ordering.API.Domain.OrderAggregate;
using Ordering.API.Domain.OrderAggregate.Events;

namespace Ordering.Test.Application
{
    public class DomainEventTests
    {
        [Fact]
        public async Task OrderInitiated_ShouldPublishOrderConfirmStockAsync()
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

            var producerMock = new Mock<IExternalEventProducer>();
            producerMock.Setup(x => x.PublishAsync(
                It.IsAny<IntegrationEvent>(), 
                It.IsAny<CancellationToken>())
            ).Returns(Task.CompletedTask);

            var repositoryMock = new Mock<IOrderRepository>();
            repositoryMock.Setup(x => x.FindAsync(
                It.IsAny<Guid>(), 
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(order);

            var handler = new OrderInitiatedHandler(producerMock.Object, repositoryMock.Object);

            var evt = order.Events.First() as OrderInitiated;
            await handler.HandleAsync(evt, TestContext.Current.CancellationToken);

            producerMock.Verify(
                p => p.PublishAsync(
                It.Is<OrderConfirmStockIntegrationEvent>(e => e.OrderId == evt.Id),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }

        [Fact]
        public async Task OrderInitiated_WhenOrderNotFound_ShouldNotPublish()
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

            var producerMock = new Mock<IExternalEventProducer>();

            var repositoryMock = new Mock<IOrderRepository>();
            repositoryMock.Setup(x => x.FindAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync((Order?)null);

            var handler = new OrderInitiatedHandler(producerMock.Object, repositoryMock.Object);

            var evt = order.Events.First() as OrderInitiated;
            await handler.HandleAsync(evt, TestContext.Current.CancellationToken);

            producerMock.Verify(
                p => p.PublishAsync(
                It.IsAny<IntegrationEvent>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
        }
    }
}
