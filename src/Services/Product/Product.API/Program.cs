using Core.AspNet.Endpoints;
using Core.AspNet.Extensions;
using Core.Autofac;
using Core.MediaR;
using Core.MongoDB;
using Core.MongoDB.Context;
using FastEndpoints;
using FastEndpoints.Swagger;
using MediatR;
using MongoDB.Bson.Serialization.Conventions;
using OpenTelemetry.Resources;
using Product.API.Application.Product.Get;
using Product.API.Application.Product.Update;
using Product.API.Infrastructure;
using System.Reflection;

var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddFastEndpoints()
//    .AddSwaggerGen()
//    .SwaggerDocument();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(rb => rb.AddService("Product.API"));
builder.AddMongoTelemetry();
builder.AddServiceDefaults();
builder.AddAutofac();
builder.Services.AddMongoDbContext<AppDbContext>(options =>
{
    options.Connection = builder.Configuration.GetSection("Mongo:Connection").Get<MongoConnectionOptions>()!;
    options.Telemetry.Enable = true;
});
builder.Services.AddGrpc();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
});

var app = builder.Build();

app.UseServiceDefaults();
app.UseHttpsRedirection();
//app.UseFastEndpoints(config => config.DefaultResponseConfigs())
//    .UseSwaggerGen();

app.MapGrpcService<GetProduct>();
app.MapGrpcService<UpdateProduct>();

await app.RunAsync();