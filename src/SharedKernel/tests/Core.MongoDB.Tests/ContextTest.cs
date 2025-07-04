using Core.MongoDB.Context;
using Core.MongoDB.Repository;
using Core.MongoDB.Tests.Fixtures;
using MediatR;
using MongoDB.Bson;
using Moq;

namespace Core.MongoDB.Tests
{
    public class ContextTest: IClassFixture<MongoContextFixture<TestDbContext>>
    {
        private readonly TestDbContext _context;

        private readonly ProductRepository _productRepository;
        private readonly UnitOfWork _unitOfWork;

        public ContextTest(MongoContextFixture<TestDbContext> fixture) 
        {
            _context = fixture.DbContext;
            _productRepository = new ProductRepository(_context);
            _unitOfWork = new UnitOfWork(_context);
        }

        [Fact]
        public async Task add_entity_success()
        {
            var products = DummyData();
            _productRepository.Add(products.ElementAt(0));

            var rsAdd = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            Assert.True(rsAdd);
        }

        [Fact]
        public async Task save_entity_success()
        {
            var products = DummyData();
            _productRepository.AddRange(products);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            var product = products.ElementAt(0);
            product.Qty = 2;
            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            var updatedProduct = await _productRepository.FindAsync(product.Id);
            Assert.Equal(product.Qty, updatedProduct?.Qty);
        }

        private static IEnumerable<Product> DummyData() => new List<Product>()
        {
            new() { Id = ObjectId.GenerateNewId(), Name = "Test 1", Qty = 1 },
            new() { Id = ObjectId.GenerateNewId(), Name = "Test 2", Qty = 2 },
            new() { Id = ObjectId.GenerateNewId(), Name = "Test 3", Qty = 3 },
            new() { Id = ObjectId.GenerateNewId(), Name = "Test 4", Qty = 4 },
            new() { Id = ObjectId.GenerateNewId(), Name = "Test 5", Qty = 5 },
        };
    }

    public class ProductRepository : Repository<Product>
    {
        public ProductRepository(MongoContext context) : base(context)
        {
        }
    }
}