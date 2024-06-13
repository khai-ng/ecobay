using Core.Autofac;
using Core.MongoDB.Context;
using Core.MongoDB.Repository;
using ProductAggregate.API.Application.Abstractions;
using ProductAggregate.API.Domain.ProductAggregate;

namespace ProductAggregate.API.Infrastructure
{
    public class ProductRespository : Repository<ProductItem>, IProductRepository, ITransient
    {
        private readonly IMongoContext _context;
        public ProductRespository(IMongoContext context) : base(context)
        {
            _context = context;
            SetCollection("origin_product");
        }
    }
}