using Core.EntityFramework.Context;
using Core.EntityFramework.Repositories;
using Core.EntityFramework.IntegrationTests.Fixtures;
using Core.Events.DomainEvents;
using Core.Mediator;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Core.EntityFramework.IntegrationTests
{
    public record ProductCreated(Guid AggregateId) : DomainEvent<Guid>(AggregateId);

    public class RepositoryTests : IClassFixture<EfCorePostgreFixture<TestDbContext>>
    {
        private readonly TestDbContext _context;

        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductRepository _productRepository;
        private readonly UnitOfWork _unitOfWork;

        public RepositoryTests(EfCorePostgreFixture<TestDbContext> fixture)
        {
            _context = fixture.DbContext;
            _mediatorMock = new Mock<IMediator>();
            _productRepository = new ProductRepository(_context);
            _unitOfWork = new UnitOfWork(_context, _mediatorMock.Object);
        }

        [Fact]
        public async ValueTask AddProduct_ShouldReturnTrueAsync()
        {
            _mediatorMock.Setup(x => x.PublishAsync(It.IsAny<IRequest>(), default))
                .Returns(Task.FromResult(true));

            var products = DummyData();
            _productRepository.Add(products.ElementAt(0));

            var rsAdd = await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);
            Assert.True(rsAdd);
        }

        [Fact]
        public async ValueTask UpdateProduct_ShouldPersistQuantityAsync()
        {
            _mediatorMock.Setup(x => x.PublishAsync(It.IsAny<IRequest>(), default))
                .Returns(Task.FromResult(true));

            var products = DummyData();
            await _productRepository.BulkAddAsync(products);

            var product = products.ElementAt(0);
            product.Qty = 2;
            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            var updatedProduct = await _productRepository.FindAsync(product.Id);
            Assert.Equal(product.Qty, updatedProduct?.Qty);
        }

        [Fact]
        public async ValueTask RemoveProduct_ShouldNoLongerBeFoundAsync()
        {
            _mediatorMock.Setup(x => x.PublishAsync(It.IsAny<IRequest>(), default))
                .Returns(Task.FromResult(true));

            var product = DummyData().First();
            _productRepository.Add(product);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            _productRepository.Remove(product);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            var found = await _productRepository.FindAsync(product.Id);
            Assert.Null(found);
        }

        [Fact]
        public async ValueTask AddRange_ShouldPersistAllProductsAsync()
        {
            _mediatorMock.Setup(x => x.PublishAsync(It.IsAny<IRequest>(), default))
                .Returns(Task.FromResult(true));

            var products = DummyData().ToList();
            _productRepository.AddRange(products);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            var all = await _productRepository.GetAllAsync();
            Assert.True(all.Count() >= products.Count);
        }

        [Fact]
        public async ValueTask RemoveRange_ShouldRemoveAllSpecifiedProductsAsync()
        {
            _mediatorMock.Setup(x => x.PublishAsync(It.IsAny<IRequest>(), default))
                .Returns(Task.FromResult(true));

            var products = DummyData().ToList();
            await _productRepository.BulkAddAsync(products);

            _productRepository.RemoveRange(products);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            foreach (var product in products)
            {
                var found = await _productRepository.FindAsync(product.Id);
                Assert.Null(found);
            }
        }

        [Fact]
        public async ValueTask FindAsync_WithSelector_ShouldReturnProjectedNameAsync()
        {
            _mediatorMock.Setup(x => x.PublishAsync(It.IsAny<IRequest>(), default))
                .Returns(Task.FromResult(true));

            var product = DummyData().First();
            _productRepository.Add(product);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            var name = await _productRepository.FindAsync(product.Id, x => x.Name);
            Assert.Equal(product.Name, name);
        }

        [Fact]
        public async ValueTask GetAllAsync_ShouldReturnPersistedProductsAsync()
        {
            _mediatorMock.Setup(x => x.PublishAsync(It.IsAny<IRequest>(), default))
                .Returns(Task.FromResult(true));

            var products = DummyData().ToList();
            _productRepository.AddRange(products);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            var all = await _productRepository.GetAllAsync();
            Assert.NotEmpty(all);
        }

        [Fact]
        public async ValueTask GetAllAsync_WithSelector_ShouldReturnProjectedNamesAsync()
        {
            _mediatorMock.Setup(x => x.PublishAsync(It.IsAny<IRequest>(), default))
                .Returns(Task.FromResult(true));

            var products = DummyData().ToList();
            _productRepository.AddRange(products);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            var names = await _productRepository.GetAllAsync(x => x.Name);
            Assert.NotEmpty(names);
            Assert.All(names, n => Assert.False(string.IsNullOrEmpty(n)));
        }

        [Fact]
        public async ValueTask BulkUpdateAsync_ShouldPersistChangesAsync()
        {
            _mediatorMock.Setup(x => x.PublishAsync(It.IsAny<IRequest>(), default))
                .Returns(Task.FromResult(true));

            var products = DummyData().ToList();
            await _productRepository.BulkAddAsync(products);

            foreach (var p in products)
                p.Qty = 99;

            await _productRepository.BulkUpdateAsync(products);

            foreach (var p in products)
            {
                var updated = await _productRepository.FindAsync(p.Id);
                Assert.Equal(99, updated?.Qty);
            }
        }

        [Fact]
        public async ValueTask BulkDeleteAsync_ShouldRemoveProductsAsync()
        {
            _mediatorMock.Setup(x => x.PublishAsync(It.IsAny<IRequest>(), default))
                .Returns(Task.FromResult(true));

            var products = DummyData().ToList();
            await _productRepository.BulkAddAsync(products);

            await _productRepository.BulkDeleteAsync(products);

            foreach (var p in products)
            {
                var found = await _productRepository.FindAsync(p.Id);
                Assert.Null(found);
            }
        }

        [Fact]
        public async ValueTask SaveChanges_WithDomainEvents_ShouldPublishEventsToMediatorAsync()
        {
            _mediatorMock.Setup(x => x.PublishAsync(It.IsAny<IRequest>(), default))
                .Returns(Task.FromResult(true));

            var product = DummyData().First();
            product.Enqueue(new ProductCreated(product.Id));
            _productRepository.Add(product);

            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            _mediatorMock.Verify(x => x.PublishAsync(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        private static IEnumerable<Product> DummyData() => new List<Product>()
        {
            new() { Id = Guid.CreateVersion7(), Name = "Test 1", Qty = 1 },
            new() { Id = Guid.CreateVersion7(), Name = "Test 2", Qty = 2 },
            new() { Id = Guid.CreateVersion7(), Name = "Test 3", Qty = 3 },
            new() { Id = Guid.CreateVersion7(), Name = "Test 4", Qty = 4 },
            new() { Id = Guid.CreateVersion7(), Name = "Test 5", Qty = 5 },
        };
    }

    public class ProductRepository : Repository<Product>
    {
        public ProductRepository(DbContext context) : base(context)
        {
        }
    }
}