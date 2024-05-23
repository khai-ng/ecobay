using Core.Autofac;
using Core.MongoDB.Context;
using Core.MongoDB.Repository;
using Product.API.Application.Abstractions;

namespace Product.API.Infrastructure
{
    public class ProductRespository : Repository<Domain.Product>, IProductRepository, IScoped
    {
        public ProductRespository(IMongoContext context) : base(context)
        {
        }
    }
}
