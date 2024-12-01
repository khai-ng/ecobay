using ProductAggregate.API.Domain.ProductMigration;

namespace ProductAggregate.API.Application.Common.Abstractions
{
    public interface IProductMigrationRepository: IRepository<ProductItem>
    {
    }
}
