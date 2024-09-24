using Core.MongoDB.Context;
using Core.MongoDB.Repository;
using ProductAggregate.API.Domain.ProductMigration;

namespace ProductAggregate.API.Application.Common.Abstractions
{
    public interface IProductMigrationRepository: IRepository<ProductItem>
    {
    }
}
