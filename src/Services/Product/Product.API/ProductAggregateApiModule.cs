namespace Product.API
{
    public class ProductApiModule : AppModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppDbContext>()
                .InstancePerLifetimeScope();

            //builder.RegisterType<MongoContext>()
            //    .As<IUnitOfWork>()
            //    .InstancePerDependency();

            //builder.RegisterType<HashRingManager>()
            //    .As<IHashRingManager>()
            //    .SingleInstance();
        }
    }
}
