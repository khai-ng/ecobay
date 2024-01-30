using GrpcEmployee;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi;
using Web.ApiGateway.Configurations;
using Web.ApiGateway.Constants;
using Yarp.ReverseProxy.Configuration;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Extensions;
using Yarp.ReverseProxy;
using Yarp.ReverseProxy.Model;
using Google.Protobuf.WellKnownTypes;

namespace Web.ApiGateway.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddReverseProxy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddReverseProxy().LoadFromConfig(configuration.GetRequiredSection(ConfigConstants.YARP_CONFIG));
            return services;
        }
        public static IServiceCollection AddGrpcServices(this IServiceCollection services)
        {
            services.AddGrpcClient<Employee.EmployeeClient>((services, options) =>
            {
                var employeeApi = services.GetRequiredService<IOptions<UrlsConfiguration>>().Value.GrpcEmployee;
                options.Address = new Uri(employeeApi);
            });

            return services;
        }
        
        public static void MapGetSwaggerForYarp(this IEndpointRouteBuilder endpoints, IConfiguration configuration)
        {
            var clusters = configuration.GetSection("ReverseProxy:Clusters");
            var routes = configuration.GetSection("ReverseProxy:Routes").Get<List<RouteConfig>>();
            if (clusters != null)
            {
                foreach (var child in clusters.GetChildren())
                {
                    if (child.GetSection("Swagger").Exists())
                    {
                        var cluster = child.Get<ClusterConfig>();
                        var swagger = child.GetSection("Swagger").Get<GatewaySwaggerSpec>();

                        endpoints.MapSwaggerSpecs(routes!, cluster!, swagger!);
                    }
                }
            }
        }

        public static void MapSwaggerSpecs(this IEndpointRouteBuilder endpoints, List<RouteConfig> config, ClusterConfig cluster, GatewaySwaggerSpec swagger)
        {
            endpoints.MapGet(swagger.Endpoint, async (context) =>
            { 
                var client = new HttpClient();
                var root = cluster.Destinations!.First().Value.Address;

                var stream = await client.GetStreamAsync($"{root.TrimEnd('/')}/{swagger.Spec.TrimStart('/')}");

                var document = new OpenApiStreamReader().Read(stream, out var diagnostic);
                var rewrite = new OpenApiPaths();

                var routes = config.Where(p => p.ClusterId == cluster.ClusterId);
                var hasCatchAll = routes != null && routes.Any(p => p.Match.Path!.Contains("**catch-all"));

                //1: support single server
                //if(document.Servers != null)
                //{
                //    document.Servers[0].Url = string.Concat(context.Request.Scheme,
                //    "://",
                //    context.Request.Host.ToUriComponent());
                //}

                //2: remove server option from other services
                document.Servers = null;

                foreach (var path in document.Paths)
                {
                    var rewritedPath = path.Key;

                    if (hasCatchAll || routes!.Any(p => p.Match.Path!.Equals(rewritedPath) && p.Match.Methods == null))
                    {
                        rewrite.Add(rewritedPath, path.Value);
                    }
                    else
                    {
                        var routeThatMatchPath = routes!.Any(p => p.Match.Path!.Equals(rewritedPath));
                        if (routeThatMatchPath)
                        {
                            var operationToRemoves = new List<OperationType>();
                            foreach (var operation in path.Value.Operations)
                            {
                                var hasRoute = routes!.Any(
                                    p => p.Match.Path!.Equals(rewritedPath) && p.Match.Methods!.Contains(operation.Key.ToString().ToUpperInvariant())
                                );

                                if (!hasRoute)
                                {
                                    operationToRemoves.Add(operation.Key);
                                }
                            }

                            foreach (var operationToRemove in operationToRemoves)
                            {
                                path.Value.Operations.Remove(operationToRemove);
                            }

                            if (path.Value.Operations.Any())
                            {
                                rewrite.Add(rewritedPath, path.Value);
                            }
                        }
                    }
                }

                document.Paths = rewrite;

                var result = document.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Json);
                await context.Response.WriteAsync(result);
            });
        }

        public class GatewaySwaggerSpec
        {
            public required string Endpoint { get; set; }
            public required string Spec { get; set; }
            public string? OriginPath { get; set; }
            public string? TargetPath { get; set; }
        }
    }
}
