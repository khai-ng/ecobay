using Core.AspNet.Extensions;
using Core.Autofac;
using Core.MediaR;
using Core.MongoDB.Context;
using MediatR;
using MongoDB.Bson.Serialization.Conventions;
using Product.API.Application.Product;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddFastEndpoints()
//    .AddSwaggerGen()
//    .SwaggerDocument();

builder.AddServiceDefaults();
builder.AddAutofac();

builder.Services.AddGrpc();

builder.Services.Configure<MongoDbSetting>(
    builder.Configuration.GetSection("ProductDatabase"));
var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

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

app.MapGrpcService<ProductService>();

await app.RunAsync();