using Core.EntityFramework.Context;
using Core.EntityFramework.Repositories;
using Core.EntityFramework.Tests.Fixtures;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Core.EntityFramework.Tests
{
    public class ContextTest : IClassFixture<EfCorePostgreFixture<TestDbContext>>
    {
        private readonly TestDbContext _context;

        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductRepository _productRepository;
        private readonly UnitOfWork _unitOfWork;

        public ContextTest(EfCorePostgreFixture<TestDbContext> fixture)
        {
            _context = fixture.DbContext;
            _mediatorMock = new Mock<IMediator>();
            _productRepository = new ProductRepository(_context);
            _unitOfWork = new UnitOfWork(_context, _mediatorMock.Object);
        }

        [Fact]
        public async ValueTask add_entity_success()
        {
            _mediatorMock.Setup(x => x.Publish(It.IsAny<INotification>(), default))
                .Returns(Task.FromResult(true));

            var products = DummyData();
            _productRepository.Add(products.ElementAt(0));

            var rsAdd = await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);
            Assert.True(rsAdd);
        }

        [Fact]
        public async ValueTask save_entity_success()
        {
            _mediatorMock.Setup(x => x.Publish(It.IsAny<INotification>(), default))
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

        private static IEnumerable<Product> DummyData() => new List<Product>()
        {
            new() { Id = Guid.NewGuid(), Name = "Test 1", Qty = 1 },
            new() { Id = Guid.NewGuid(), Name = "Test 2", Qty = 2 },
            new() { Id = Guid.NewGuid(), Name = "Test 3", Qty = 3 },
            new() { Id = Guid.NewGuid(), Name = "Test 4", Qty = 4 },
            new() { Id = Guid.NewGuid(), Name = "Test 5", Qty = 5 },
        };
    }

    public class ProductRepository : Repository<Product>
    {
        public ProductRepository(DbContext context) : base(context)
        {
        }
    }
}