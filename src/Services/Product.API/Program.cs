using FastEndpoints;
using Product.API.Infrastructure;
using Core.AspNet.Extensions;
using Core.Autofac;
using FastEndpoints.Swagger;
using MediatR;
using System.Reflection;
using Core.MediaR;
using MongoDB.Bson.Serialization.Conventions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ProductDbSetting>(
    builder.Configuration.GetSection("ProductDatabase"));

var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();

builder.AddServiceDefaults();
builder.AddAutofac();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
});

var app = builder.Build();

app.UseServiceDefaults();
app.UseFastEndpoints(config => config.CommonResponseConfigs())
    .UseSwaggerGen();

await app.RunAsync();