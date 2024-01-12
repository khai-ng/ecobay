using Autofac;
using Autofac.Core;
using Autofac.Diagnostics;
using System.Reflection;
using System.Text;

namespace Core.Dependency
{
    public static class AssemblyRegistrar
    {
        public static void AutofacRegister(this ContainerBuilder builder)
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
                    .Where(a => !string.Equals(a.FullName, "System.Data.SqlClient, Version=4.6.1.1, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", StringComparison.OrdinalIgnoreCase))
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
