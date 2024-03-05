using Autofac;
using Kernel.Behaviors;
using ServiceDefaults;
using SharedKernel.Kernel.Module;

namespace Identity.API
{
    public class IdentityApiModule: AppModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InternalExceptionHandler>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<HttpContextEnricher>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
