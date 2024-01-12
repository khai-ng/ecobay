using Autofac;
using Core.Module;
using MediatR;

namespace EmployeeManagement.API
{
    public class EmployeeManagementApiModule : AppModule
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
