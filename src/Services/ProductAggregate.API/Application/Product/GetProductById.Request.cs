using Core.Result.AppResults;
using MediatR;
using MongoDB.Bson;

namespace ProductAggregate.API.Application.Product
{
    public class GetProductByIdRequest: IRequest<AppResult<IEnumerable<GrpcProduct.ProductItemResponse>>>
    {
        public IEnumerable<string> Ids { get; set; }
    }
}
