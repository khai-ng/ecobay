using Autofac;
using Core.Autofac;
using Core.SharedKernel;

namespace Identity.Infrastructure
{
    public class IdentityInfrastructureModule: AppModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<ClaimAuthorizationHandler>()
            //    .As<IAuthorizationHandler>()
            //    .SingleInstance();

            //builder.RegisterType<AppDbContext>()
            //    .As<IUnitOfWork>()
            //    .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
