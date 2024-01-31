using Autofac;
using SharedKernel.Kernel.Module;
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
