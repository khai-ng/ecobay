namespace Product.API
{
    public class ProductApiModule : AppModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppDbContext>()
                .InstancePerLifetimeScope();

			builder.RegisterType<UnitOfWork>()
				.As<IUnitOfWork>()
				.InstancePerLifetimeScope();
		}
    }
}
