using Core.AspNet.Extensions;
using Core.Autofac;
using Core.MediaR;
using Core.MongoDB.Context;
using FastEndpoints;
using FastEndpoints.Swagger;
using MediatR;
using MongoDB.Bson.Serialization.Conventions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();

builder.AddServiceDefaults();
builder.AddAutofac();

builder.Services.Configure<MongoDbSetting>(
    builder.Configuration.GetSection("ProductDatabase"));
var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
});

//builder.Services.AddHangfireDefaults(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseServiceDefaults();
app.UseDefaultSwaggerRedirection();
app.UseFastEndpoints(config => config.CommonResponseConfigs())
    .UseSwaggerGen();

//app.UseHangfireDashboard();
//app.AddHangFireJob();

await app.RunAsync();