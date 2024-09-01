//using Core.Autofac;
//using Core.MongoDB.Context;
//using Core.MongoDB.Repository;
//using MongoDB.Bson;
//using MongoDB.Driver;
//using ProductAggregate.API.Application.Common.Abstractions;
//using ProductAggregate.API.Domain.ProductAggregate;

//namespace ProductAggregate.API.Infrastructure
//{
//    public class ProductRepository : Repository<ProductItem>, IProductRepository, ITransient
//    {
//        private readonly IMongoContext _context;
//        public ProductRepository(IMongoContext context) : base(context)
//        {
//            _context = context;
//            SetCollection("origin_product");
//        }
      
//    }
//}