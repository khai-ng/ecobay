using Core.MongoDB.Repository;

namespace Product.API.Application.Abstractions
{
    public interface IProductRepository: IQueryRepository<Domain.ProductAggregate.Product>
    {

    }
}
