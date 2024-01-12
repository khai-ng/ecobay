using Autofac;
using Core.Module;
using MediatR;

namespace Identity.API
{
    public class IdentityApiModule: AppModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
