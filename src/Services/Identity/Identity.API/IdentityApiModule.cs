using Autofac;
using Core.Autofac;
using Core.EntityFramework.Context;
using Core.SharedKernel;

namespace Identity.API
{
    public class IdentityApiModule: AppModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
        }
    }
}
