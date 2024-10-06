using Core.AspNet.Extensions;
using Core.Autofac;
using Core.Kafka;
using Core.MediaR;
using Core.MongoDB;
using Core.MongoDB.Context;
using FastEndpoints;
using FastEndpoints.Swagger;
using Hangfire;
using MediatR;
using MongoDB.Bson.Serialization.Conventions;
using ProductAggregate.API.Application.Common.Abstractions;
using ProductAggregate.API.Infrastructure;
using ProductAggregate.API.Infrastructure.Repositories;
using ProductAggregate.API.Presentation.Configurations;
using System.Reflection;

var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();
builder.AddKafkaOpenTelemetry();
builder.AddMongoTelemetry();
builder.AddServiceDefaults();
builder.AddAutofac();
builder.Services.AddMongoDbContext<AppDbContext>(options =>
{ 
    options.Connection = builder.Configuration.GetSection("ProductDatabase").Get<MongoConnectionOptions>()!;
    options.Telemetry.Enable = true;
});
builder.Services.AddKafkaCompose();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
});

//builder.Services.AddTransient<IProductMigrationRepository, ProductMigrationRepository>();
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