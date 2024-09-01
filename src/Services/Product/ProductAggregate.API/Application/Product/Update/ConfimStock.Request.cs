using Core.Result.AppResults;
using MediatR;
using ProductAggregate.API.Application.Dto.Product;

namespace ProductAggregate.API.Application.Product.Update
{

    public class ConfimStockRequest(IEnumerable<ProductUnit> productUnit) : IRequest<AppResult>
    {
        public IEnumerable<ProductUnit> ProductUnits { get; set; } = productUnit;
    }
}