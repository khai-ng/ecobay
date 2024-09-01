using Autofac;
using Core.Autofac;
using Core.MongoDB.Context;
using Core.SharedKernel;

namespace Product.API
{
    public class ProductApiModule: AppModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MongoContext>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            //builder.RegisterType<HashRingManager>()
            //    .As<IHashRingManager>()
            //    .SingleInstance();
        }
    }
}
