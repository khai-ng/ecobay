using ProductAggregate.API.Domain.ProductMigration;

namespace ProductAggregate.API.Infrastructure.Repositories
{
    public class ProductMigrationRepository : Repository<ProductItem>, IProductMigrationRepository, ITransient
    {

        public ProductMigrationRepository(AppDbContext context) : base(context)
        {
        }

    }
}
