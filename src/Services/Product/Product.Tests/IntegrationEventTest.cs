using Core.Entities;
using Core.IntegrationEvents.IntegrationEvents;
using MongoDB.Bson;
using Moq;
using Product.API.Application.Abstractions;
using Product.API.Application.IntegrationEvents;
using Product.API.Application.Product;
using Product.API.Domain.ProductAggregate;

namespace Product.Tests
{
    public class IntegrationEventTest
    {
        private readonly Mock<IIntegrationProducer> _producerMock = new();
        private readonly Mock<IUnitOfWork> _uowMock = new();
        public IntegrationEventTest() 
        {
            _producerMock.Setup(x => x.PublishAsync(
                It.IsAny<IntegrationEvent>(),
                It.IsAny<CancellationToken>())
            ).Returns(Task.CompletedTask);

            _uowMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
        }

        [Fact]
        public async void valid_request_should_publish_success_event()
        {
            var data = DummyData().Where(x => x.MainCategory == "Cate1").ToList();

            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<IEnumerable<ObjectId>>()))
                .ReturnsAsync(data);

            var evt = new OrderConfirmStockIntegrationEvent(Guid.NewGuid(), new List<ProductQtyDto>()
            {
                new(data[0].Id.ToString(), 2),
                new(data[1].Id.ToString(), 3),
            });
            var handler = new OrderConfirmStockIntegrationEventHandler(_producerMock.Object, repositoryMock.Object, _uowMock.Object);

            await handler.HandleAsync(evt);

            _producerMock.Verify(
                p => p.PublishAsync(
                It.Is<OrderConfirmStockSuccessIntegrationEvent>(e => e.OrderId == evt.OrderId),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }

        [Fact]
        public async void out_of_stock_should_publish_failed_event()
        {
            var data = DummyData().Where(x => x.MainCategory == "Cate1").ToList();

            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<IEnumerable<ObjectId>>()))
                .ReturnsAsync(data);

            var evt = new OrderConfirmStockIntegrationEvent(Guid.Empty, new List<ProductQtyDto>()
            {
                new(data[0].Id.ToString(), 2),
                new(data[1].Id.ToString(), 5),
            });
            var handler = new OrderConfirmStockIntegrationEventHandler(_producerMock.Object, repositoryMock.Object, _uowMock.Object);

            await handler.HandleAsync(evt);

            _producerMock.Verify(
                p => p.PublishAsync(
                It.Is<OrderConfirmStockFailedIntegrationEvent>(e => e.OrderId == evt.OrderId),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }

        [Fact]
        public async void not_found_should_publish_failed_event()
        {
            var data = DummyData().Where(x => x.MainCategory == "Cate1").ToList();

            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<IEnumerable<ObjectId>>()))
                .ReturnsAsync(data);

            var evt = new OrderConfirmStockIntegrationEvent(Guid.Empty, new List<ProductQtyDto>()
            {
                new(ObjectId.GenerateNewId().ToString(), 1),
            });
            var handler = new OrderConfirmStockIntegrationEventHandler(_producerMock.Object, repositoryMock.Object, _uowMock.Object);

            await handler.HandleAsync(evt);

            _producerMock.Verify(
                p => p.PublishAsync(
                It.Is<OrderConfirmStockFailedIntegrationEvent>(e => e.OrderId == evt.OrderId),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }


        private IEnumerable<ProductItem> DummyData()
        {
            return new List<ProductItem>()
            {
                new() { Id = ObjectId.GenerateNewId(), Title = "Test 1", MainCategory = "Cate1", Qty = 2 },
                new() { Id = ObjectId.GenerateNewId(), Title = "Test 2", MainCategory = "Cate1", Qty = 4 },
                new() { Id = ObjectId.GenerateNewId(), Title = "Test 3", MainCategory = "Cate2", Qty = 4 },
                new() { Id = ObjectId.GenerateNewId(), Title = "Test 4", MainCategory = "Cate2", Qty = 6 },
                new() { Id = ObjectId.GenerateNewId(), Title = "Test 5", MainCategory = "Cate2", Qty = 8 },
            };
        }
    }
}
