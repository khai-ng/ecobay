using Autofac;
using System.Reflection;

namespace Core.Dependency
{
    public static class DependencyExtension
    {
        public static void AutofacGenericRegisterBuilder<TLifeTime>(this ContainerBuilder builder, Assembly assembly)
            where TLifeTime : class
        {
            var implement = builder.RegisterAssemblyOpenGenericTypes(assembly)
                .Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(TLifeTime))))
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

        public static void AutofacRegisterBuilder<TLifeTime>(this ContainerBuilder builder, Assembly assembly)
            where TLifeTime : class
        {

			var implement = builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(TLifeTime))))
                .PropertiesAutowired((propertyInfo, instance)
                    => propertyInfo.GetCustomAttribute<PropertyDependency>() != null)
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

        public static void AutofacTypeRegisterBuilder<TLifeTime>(this ContainerBuilder builder, Type type)
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

        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
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
    }
}
