namespace ProductAggregate.API
{
    public class ProductAggregateApiModule : AppModule
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
