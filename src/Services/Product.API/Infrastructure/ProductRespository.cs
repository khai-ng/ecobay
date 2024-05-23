using Core.Autofac;
using Core.MongoDB.Context;
using Core.MongoDB.Repository;
using MongoDB.Driver;
using Product.API.Application.Abstractions;

namespace Product.API.Infrastructure
{
    public class ProductRespository : Repository<Domain.ProductAggregate.Product>, IProductRepository, IScoped
    {
        private readonly IMongoContext _context;
        public ProductRespository(IMongoContext context) : base(context)
        {
            _context = context;
        }

        protected override IMongoCollection<Domain.ProductAggregate.Product> Collection 
            => _context.GetCollection<Domain.ProductAggregate.Product>("product");
    }
}