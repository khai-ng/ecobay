using Core.Pagination;
using MongoDB.Bson;
using Moq;
using Product.API.Application.Abstractions;
using Product.API.Application.Product;
using Product.API.Application.Product.Get;
using Product.API.Domain.ProductAggregate;
using Product.API.Infrastructure;

namespace Product.Tests
{
    public class ProductTest
    {

        [Fact]
        public async ValueTask get_success()
        {
            var data = DummyData();

            var request = new GetProductCommand("Cate2", 1, 2);
            
            var query = new GetProductQuery(request.Category, request.PageIndex, request.PageSize);
            var response = FluentPaging
                .From(query)
                .Paging(data.Select(x => new ProductItemDto()
                {
                    Id = x.Id.ToString(),
                    MainCategory = x.MainCategory,
                    Title = x.Title,
                }));

            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(r => r.GetPagingAsync(
                It.IsAny<GetProductQuery>(),
                It.IsAny<Func<ProductItem, ProductItemDto>>())
            ).ReturnsAsync(response);

            var handler = new GetProductHandler(repositoryMock.Object);
            var result = await handler.HandleAsync(request, CancellationToken.None);

            Assert.True(result.IsSuccess);      
        }

        [Fact]
        public async ValueTask get_by_id_success()
        {
            var data = DummyData().Where(x => x.MainCategory == "Cate2");

            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<IEnumerable<ObjectId>>()))
                .ReturnsAsync(data);

            var request = new GetProductByIdCommand(data.Select(x => x.Id.ToString()));
            var handler = new GetProductByIdHandler(repositoryMock.Object);
            var result = await handler.HandleAsync(request, CancellationToken.None);

            Assert.True(result.IsSuccess);
            for (int i = 0; i < result.Data?.Count(); i++)
            {
                Assert.Equal(data.ElementAt(i).Id.ToString(), result.Data?.ElementAt(i).Id);
            }
        }

        private IEnumerable<ProductItem> DummyData()
        {
            return new List<ProductItem>()
            {
                new() { Id = ObjectId.GenerateNewId(), Title = "Test 1", MainCategory = "Cate1" },
                new() { Id = ObjectId.GenerateNewId(), Title = "Test 2", MainCategory = "Cate1" },
                new() { Id = ObjectId.GenerateNewId(), Title = "Test 3", MainCategory = "Cate2" },
                new() { Id = ObjectId.GenerateNewId(), Title = "Test 4", MainCategory = "Cate2" },
                new() { Id = ObjectId.GenerateNewId(), Title = "Test 5", MainCategory = "Cate2" },
            };
        }


    }
}
