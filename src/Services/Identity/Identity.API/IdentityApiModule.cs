using Autofac;
using Core.Autofac;

namespace Identity.API
{
    public class IdentityApiModule: AppModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<InternalExceptionHandler>()
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
