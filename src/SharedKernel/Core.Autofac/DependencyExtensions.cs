using Autofac;
using Autofac.Core;
using Autofac.Diagnostics;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text;

namespace Core.Autofac
{
    public static class DependencyExtensions
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
                    .Where(a => !string.Equals(a.FullName, "Microsoft.Data.SqlClient, Version=5.0.0.0, Culture=neutral, PublicKeyToken=23ec7fc2d6eaa4a5", StringComparison.OrdinalIgnoreCase))
                    //.Where(a => !string.Equals(a.FullName, "Pomelo.EntityFrameworkCore.MySql, Version=7.0.0.0, Culture=neutral, PublicKeyToken=2cc498582444921b", StringComparison.OrdinalIgnoreCase))
                    .ToList();
                foreach (var item in applicationAsseblies)
                {
                    var moduleType = item?.GetTypes()
                        .Where(p => typeof(IModule).IsAssignableFrom(p) && !p.IsAbstract)
                        .ToList();

                    if (moduleType is null)
                        continue;

                    moduleTypes.AddRange(moduleType);
                }
                return moduleTypes;
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
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
            ////Generic
            builder.AutofacGenericRegisterBuilder<ITransient>(module);
            builder.AutofacGenericRegisterBuilder<IScoped>(module);
            builder.AutofacGenericRegisterBuilder<ISingleton>(module);

            //Normal
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

            if (typeof(TLifeTime).Equals(typeof(ITransient)))
            {
                implement.InstancePerDependency();
                return;
            }
            if (typeof(TLifeTime).Equals(typeof(IScoped)))
            {
                implement.InstancePerLifetimeScope();
                return;
            }
            if (typeof(TLifeTime).Equals(typeof(ISingleton)))
            {
                implement.SingleInstance();
                return;
            }
        }

        private static void AutofacRegisterBuilder<TLifeTime>(this ContainerBuilder builder, Assembly assembly)
            where TLifeTime : class
        {

			var implement = builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(TLifeTime))))
                .PropertiesAutowired((propertyInfo, instance)
                    => propertyInfo.GetCustomAttribute<PropertyDependency>() != null)
                .AsImplementedInterfaces();

            if (typeof(TLifeTime).Equals(typeof(ITransient)))
            {
                implement.InstancePerDependency();
                return;
            }
            if (typeof(TLifeTime).Equals(typeof(IScoped)))
            {
                implement.InstancePerLifetimeScope();
                return;
            }
            if (typeof(TLifeTime).Equals(typeof(ISingleton)))
            {
                implement.SingleInstance();
                return;
            }
        }

        private static void AutofacTypeRegisterBuilder<TLifeTime>(this ContainerBuilder builder, Type type)
			where TLifeTime : class
		{
            var implement = builder.RegisterType(type)
                .AsImplementedInterfaces();

			switch (typeof(TLifeTime))
			{
				case ITransient:
					implement.InstancePerDependency();
					break;
				case IScoped:
					implement.InstancePerLifetimeScope();
					break;
				case ISingleton:
					implement.SingleInstance();
					break;
				default:
					implement.InstancePerDependency();
					break;
			}
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
