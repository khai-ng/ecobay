using Autofac;
using Core.Autofac;
using Core.MongoDB.Context;

namespace Product.API
{
    public class ProductApiModule: AppModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MongoContext>()
                .As<IMongoContext>()
                .InstancePerLifetimeScope();
        }
    }
}
