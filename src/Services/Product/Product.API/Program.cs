using Core.AspNet.Extensions;
using Core.Autofac;
using Core.MediaR;
using Core.MongoDB;
using Core.MongoDB.Context;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson.Serialization.Conventions;
using OpenTelemetry.Resources;
using Product.API.Application.Product.Get;
using Product.API.Application.Product.Update;
using Product.API.Infrastructure;
using System.Reflection;

var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddAutofac();

builder.Services
    .AddMongoTelemetry()
    .AddOpenTelemetry()
    .ConfigureResource(rb => rb.AddService("Product.API"));

builder.Services
    .AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

builder.Services
    .AddMongoDbContext<AppDbContext>(options =>
    {
        options.Connection = builder.Configuration.GetSection("Mongo:Connection").Get<MongoConnectionOptions>()!;
        options.Telemetry.Enable = true;
    })
    .AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
    })
    .AddGrpc();

var app = builder.Build();

app.UseServiceDefaults();
app.UseHttpsRedirection();

app.MapHealthChecks("/hc");
app.MapHealthChecks("/liveness", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});

app.MapGrpcService<GetProduct>();
app.MapGrpcService<UpdateProduct>();

await app.RunAsync();