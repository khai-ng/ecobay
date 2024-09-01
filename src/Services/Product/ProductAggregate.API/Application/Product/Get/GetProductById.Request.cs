using Core.Result.AppResults;
using MediatR;
using MongoDB.Bson;

namespace ProductAggregate.API.Application.Product.GetProduct
{
    public class GetProductByIdRequest : IRequest<AppResult<IEnumerable<GetProductByIdItemResponse>>>
    {
        public IEnumerable<string> Ids { get; set; }
    }
}
