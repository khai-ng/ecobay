using Core.EntityFramework.Context;
using Core.EntityFramework.Pagination;
using Core.EntityFramework.IntegrationTests.Fixtures;
using Core.Mediator;
using Core.Pagination;
using Moq;

namespace Core.EntityFramework.IntegrationTests
{
    public class FluentPagingTests: IClassFixture<EfCorePostgreFixture<TestDbContext>>
    {
        private readonly TestDbContext _context;

        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductRepository _productRepository;
        private readonly UnitOfWork _unitOfWork;

        public FluentPagingTests(EfCorePostgreFixture<TestDbContext> fixture)
        {
            _context = fixture.DbContext;
            _mediatorMock = new Mock<IMediator>();
            _productRepository = new ProductRepository(_context);
            _unitOfWork = new UnitOfWork(_context, _mediatorMock.Object);
        }

        [Fact]
        public async ValueTask Paging_FromRequest_ShouldReturnPageOfProductsAsync()
        {
            _mediatorMock.Setup(x => x.PublishAsync(It.IsAny<IRequest>(), default))
                .Returns(Task.FromResult(true));
            var products = DummyData();
            _productRepository.AddRange(products);
            _ = await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            var request = new PagingRequest(1, 2);
            var rs = await CountedFluentPaging.From(request).PagingAsync(_context.Set<Product>());
            
            Assert.NotNull(rs.Data);
            Assert.Equal(2, rs.Data.Count());   
        }

        [Fact]
        public async ValueTask Paging_FilterApply_ShouldReturnProjectedPageAsync()
        {
            _mediatorMock.Setup(x => x.PublishAsync(It.IsAny<IRequest>(), default))
                .Returns(Task.FromResult(true));
            var products = DummyData();
            _productRepository.AddRange(products);
            _ = await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);


            var request = new PagingRequest(1, 2);
            var fluentPaging = CountedFluentPaging.From(request);
            var filter = fluentPaging.FilterApply(_context.Set<Product>());

            var rs = fluentPaging.Result(filter.Select(x => x.Name));

            Assert.NotNull(rs.Data);
            Assert.Equal(2, rs.Data.Count());
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
}
