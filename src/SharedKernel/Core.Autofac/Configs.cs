using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Diagnostics;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.Scanning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text;

namespace Core.Autofac
{
    public static class Configs
    {
        public static WebApplicationBuilder AddAutofac(this WebApplicationBuilder builder)
        {
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>((_, container) =>
            {
                container.AutofacRegister();
                container.Populate(new ServiceCollection());
            });

            return builder;
        } 

        private static void AutofacRegister(this ContainerBuilder builder)
        {
            var listModule = GetListModule();
            listModule?.ForEach(type =>
            {
                builder.AbstractDependencyRegister(type.Assembly);
                builder.RegisterModule((IModule)Activator.CreateInstance(type)!);
            });

            //builder.AutoFacLogging();
        }

        private static List<Type>? GetListModule()
        {
            List<Type>? moduleTypes = new();
            try
            {
                const string assembliesFetchPattern = "*.dll";

                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (string.IsNullOrWhiteSpace(path))
                    return null;

                var applicationAsseblies = Directory
                    .GetFiles(path, assembliesFetchPattern, SearchOption.TopDirectoryOnly)
                    .Select(Assembly.LoadFrom)
                    .ToList();
                foreach (var item in applicationAsseblies)
                {
                    var moduleType = item?.GetTypes()
                        .Where(p => typeof(IModule).IsAssignableFrom(p) && !p.IsAbstract)
                        .ToList();

                    if (moduleType is null || moduleType.Count == 0)
                        continue;

                    moduleTypes.AddRange(moduleType);
                }
                return moduleTypes;
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException? exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                //Display or log the error based on your application.
                return null;
            }
        }

        private static void AbstractDependencyRegister(this ContainerBuilder builder, Assembly module)
        {
            builder.AutofacGenericRegisterBuilder<ITransient>(module);
            builder.AutofacGenericRegisterBuilder<IScoped>(module);
            builder.AutofacGenericRegisterBuilder<ISingleton>(module);

            builder.AutofacRegisterBuilder<ITransient>(module);
            builder.AutofacRegisterBuilder<IScoped>(module);
            builder.AutofacRegisterBuilder<ISingleton>(module);
        }

		private static void AutofacGenericRegisterBuilder<TLifeTime>(this ContainerBuilder builder, Assembly assembly)
			where TLifeTime : class
		{
			var implement = builder.RegisterAssemblyOpenGenericTypes(assembly)
				.Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(TLifeTime))))
				.AsImplementedInterfaces();

			implement.ApplyLifetime<TLifeTime, object, OpenGenericScanningActivatorData, DynamicRegistrationStyle>();
		}

		private static void AutofacRegisterBuilder<TLifeTime>(this ContainerBuilder builder, Assembly assembly)
			where TLifeTime : class
		{

            var implement = builder.RegisterAssemblyTypes(assembly)
				.Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(TLifeTime))))
				.AsImplementedInterfaces();

			implement.ApplyLifetime<TLifeTime, object, ScanningActivatorData, DynamicRegistrationStyle>();
		}

		private static void ApplyLifetime<TLifeTime, TLimit, TActivatorData, TRegistrationStyle>(
			this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> implement)
			where TLifeTime : class
		{
			if (typeof(TLifeTime).Equals(typeof(ITransient)))
				implement.InstancePerDependency();
			else if (typeof(TLifeTime).Equals(typeof(IScoped)))
				implement.InstancePerLifetimeScope();
			else if (typeof(TLifeTime).Equals(typeof(ISingleton)))
				implement.SingleInstance();
		}

        private static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type? baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }

        private static void AutoFacLogging(this ContainerBuilder builder)
        {
            var tracer = new DefaultDiagnosticTracer();
            tracer.OperationCompleted += (sender, args) =>
            {
                Console.WriteLine(args.TraceContent);
            };

            builder.RegisterBuildCallback(c =>
            {
                var container = c as IContainer;
                container?.SubscribeToDiagnostics(tracer);
            });
        }
    }
}
