using Core.Result.AppResults;
using MediatR;
using MongoDB.Bson;

namespace Product.API.Application.Product
{
    public class GetProductByIdRequest : IRequest<AppResult<IEnumerable<GetProductResponse>>>
    {
        public IEnumerable<ObjectId> Ids { get; set; }
    }
}
