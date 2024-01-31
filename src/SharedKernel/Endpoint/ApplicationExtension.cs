using Autofac;
using Autofac.Extensions.DependencyInjection;
using Endpoint.Attributes;
using Infrastructure.Kernel.Dependency;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Infrastructure.Endpoint
{
    public static class ApplicationExtension
    {
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
        {
            var listEndpoint = GetListEndpoint();

            foreach (var endpoint in listEndpoint)
            {
                var endpointObj = app.ServiceProvider.GetRequiredService(endpoint);
                var handlerInfo = endpoint.GetMethod(((IBaseEndpoint)endpointObj).Handler);
                var handler = handlerInfo!.CreateDelegate(endpointObj);

                var verbAttribute = (HttpAttribute)Attribute.GetCustomAttribute(endpoint, typeof(HttpAttribute))!;

                if (verbAttribute != null)
                {
                    var httpAttribute = verbAttribute!;
                    app.MapMethods(
                        httpAttribute.Route,
                        new[] { httpAttribute.Method.ToString() },
                        handler);
                }

                
                Console.WriteLine("hi");
            }

            return app;
        }

        private static List<Type> GetListEndpoint()
        {
            List<Type> endpointTypes = new();
            try
            {
                var endpointType = Assembly.GetEntryAssembly()!.GetTypes()
                    .Where(p => typeof(IBaseEndpoint).IsAssignableFrom(p) && !p.IsAbstract)
                    .ToList();

                if (endpointType is null)
                    return new();

                endpointTypes.AddRange(endpointType);
                
                return endpointTypes;
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
                return new();
            }
        }

        public static Delegate CreateDelegate(this MethodInfo methodInfo, object target)
        {
            Func<Type[], Type> getType;
            var isAction = methodInfo.ReturnType.Equals((typeof(void)));
            var types = methodInfo.GetParameters().Select(p => p.ParameterType);

            if (isAction)
            {
                getType = Expression.GetActionType;
            }
            else
            {
                getType = Expression.GetFuncType;
                types = types.Concat(new[] { methodInfo.ReturnType });
            }

            if (methodInfo.IsStatic)
            {
                return Delegate.CreateDelegate(getType(types.ToArray()), methodInfo);
            }

            return Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
        }
    }
}
