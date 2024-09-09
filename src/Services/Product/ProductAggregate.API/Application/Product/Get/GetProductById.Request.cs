using Core.Result.AppResults;
using MediatR;

namespace ProductAggregate.API.Application.Product.GetProduct
{
    public class GetProductByIdRequest : IRequest<AppResult<IEnumerable<ProductItemDto>>>
    {
        public IEnumerable<string> Ids { get; set; }
    }
}
