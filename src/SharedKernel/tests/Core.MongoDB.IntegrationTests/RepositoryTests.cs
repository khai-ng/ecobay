using Core.MongoDB.Context;
using Core.MongoDB.Repository;
using Core.MongoDB.IntegrationTests.Fixtures;
using MongoDB.Bson;

namespace Core.MongoDB.IntegrationTests
{
    public class RepositoryTests: IClassFixture<MongoContextFixture<TestDbContext>>
    {
        private readonly TestDbContext _context;

        private readonly ProductRepository _productRepository;
        private readonly UnitOfWork _unitOfWork;

        public RepositoryTests(MongoContextFixture<TestDbContext> fixture) 
        {
            _context = fixture.DbContext;
            _productRepository = new ProductRepository(_context);
            _unitOfWork = new UnitOfWork(_context);
        }

        [Fact]
        public async ValueTask AddProduct_ShouldReturnTrueAsync()
        {
            var products = DummyData();
            _productRepository.Add(products.ElementAt(0));

            var rsAdd = await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);
            Assert.True(rsAdd);
        }

        [Fact]
        public async ValueTask UpdateProduct_ShouldPersistQuantityAsync()
        {
            var products = DummyData();
            _productRepository.AddRange(products);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

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
            var product = DummyData().First();
            _productRepository.Add(product);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            _productRepository.Remove(product);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            var found = await _productRepository.FindAsync(product.Id);
            Assert.Null(found);
        }

        [Fact]
        public async ValueTask RemoveRange_ShouldRemoveAllSpecifiedProductsAsync()
        {
            var products = DummyData().ToList();
            _productRepository.AddRange(products);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            _productRepository.RemoveRange(products);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            foreach (var product in products)
            {
                var found = await _productRepository.FindAsync(product.Id);
                Assert.Null(found);
            }
        }

        [Fact]
        public async ValueTask UpdateRange_ShouldPersistChangesForAllProductsAsync()
        {
            var products = DummyData().ToList();
            _productRepository.AddRange(products);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            foreach (var p in products)
                p.Qty = 99;

            _productRepository.UpdateRange(products);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            foreach (var p in products)
            {
                var updated = await _productRepository.FindAsync(p.Id);
                Assert.Equal(99, updated?.Qty);
            }
        }

        [Fact]
        public async ValueTask FindAsync_WithSelector_ShouldReturnProjectedNameAsync()
        {
            var product = DummyData().First();
            _productRepository.Add(product);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            var name = await _productRepository.FindAsync(product.Id, x => x.Name);
            Assert.Equal(product.Name, name);
        }

        [Fact]
        public async ValueTask GetAllAsync_ShouldReturnAllPersistedProductsAsync()
        {
            var products = DummyData().ToList();
            _productRepository.AddRange(products);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            var all = await _productRepository.GetAllAsync();
            Assert.NotEmpty(all);
        }

        [Fact]
        public async ValueTask GetAllAsync_WithSelector_ShouldReturnProjectedNamesAsync()
        {
            var products = DummyData().ToList();
            _productRepository.AddRange(products);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            var names = await _productRepository.GetAllAsync(x => x.Name);
            Assert.NotEmpty(names);
            Assert.All(names, n => Assert.False(string.IsNullOrEmpty(n)));
        }

        [Fact]
        public async ValueTask SaveChanges_MultipleQueuedCommands_ShouldAllBeExecutedAsync()
        {
            var products = DummyData().ToList();

            // Queue multiple Add commands without saving in between
            foreach (var p in products)
                _productRepository.Add(p);

            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            foreach (var p in products)
            {
                var found = await _productRepository.FindAsync(p.Id);
                Assert.NotNull(found);
            }
        }

        [Fact]
        public async ValueTask SaveChanges_CalledTwice_ShouldNotReExecutePreviousCommandsAsync()
        {
            var product = DummyData().First();
            _productRepository.Add(product);
            await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);

            // Second save with no new commands should not throw or duplicate
            var result = await _unitOfWork.SaveChangesAsync(TestContext.Current.CancellationToken);
            Assert.True(result);

            var all = await _productRepository.GetAllAsync();
            Assert.Equal(1, all.Count(x => x.Id == product.Id));
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