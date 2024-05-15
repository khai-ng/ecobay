using Core.Autofac;
using Core.Result.AppResults;
using Core.Result.Paginations;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Product.API.Infrastructure;

namespace Product.API.Application
{
    public class GetProduct : IRequestHandler<GetProductRequest, AppResult<PagingResponse<Domain.Product>>>, IScoped
    {
        IMongoCollection<Domain.Product> _productCollection;

        public GetProduct(IOptions<ProductDbSetting> productDbSettings)
        {
            var mongoClient = new MongoClient(
            productDbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                productDbSettings.Value.DatabaseName);

            _productCollection = mongoDatabase.GetCollection<Domain.Product>(
            productDbSettings.Value.CollectionName);
        }

        public async Task<AppResult<PagingResponse<Domain.Product>>> Handle(
            GetProductRequest request, 
            CancellationToken cancellationToken)
        {
            var data = await _productCollection.Find(_ => true)
                .Skip(request.Skip)
                .Limit(request.PageSize + 1)
                .ToListAsync();

            return PagingTyped.From(request).Taking(data);
        }
    }
}
