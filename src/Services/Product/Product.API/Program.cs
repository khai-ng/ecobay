using Core.AspNet.Extensions;
using Core.Autofac;
using Core.MediaR;
using Core.MongoDB.Context;
using FastEndpoints;
using FastEndpoints.Swagger;
using MediatR;
using MongoDB.Bson.Serialization.Conventions;
using Product.API.Application.Product.Get;
using Product.API.Application.Product.Update;
using System.Reflection;

var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddFastEndpoints()
//    .AddSwaggerGen()
//    .SwaggerDocument();

builder.AddServiceDefaults();
builder.AddAutofac();
builder.Services.AddGrpc();
builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection("ProductDatabase"));
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseServiceDefaults();
//app.UseFastEndpoints(config => config.CommonResponseConfigs())
//    .UseSwaggerGen();

app.MapGrpcService<GetProduct>();
app.MapGrpcService<UpdateProduct>();

await app.RunAsync();