namespace Ordering.API
{
    public class OrderingApiModule: AppModule
    {
		protected override void Load(ContainerBuilder builder)
		{
            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            base.Load(builder);
		}
	}
}
