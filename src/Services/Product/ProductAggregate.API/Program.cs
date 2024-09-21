using Autofac.Core;
using Core.AspNet.Extensions;
using Core.Autofac;
using Core.Kafka;
using Core.MediaR;
using Core.MongoDB.Context;
using Core.SharedKernel;
using FastEndpoints;
using FastEndpoints.Swagger;
using Hangfire;
using MediatR;
using MongoDB.Bson.Serialization.Conventions;
using ProductAggregate.API.Application.Common.Abstractions;
using ProductAggregate.API.Infrastructure.Repositories;
using ProductAggregate.API.Presentation.Configurations;
using System.Reflection;
using ProductAggregate.API.Infrastructure;

var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();
builder.AddKafkaOpenTelemetry();
builder.AddServiceDefaults();
builder.AddAutofac();
builder.Services.AddKafkaCompose();
builder.Services.AddHangfireDefaults(builder.Configuration);
builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection("ProductDatabase"));
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
});

builder.Services.AddTransient<IProductMigrationRepository, ProductMigrationRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseServiceDefaults();
app.UseDefaultSwaggerRedirection();
app.UseHangfireDashboard();
app.AddHangFireJob();
app.UseFastEndpoints(config => config.CommonResponseConfigs())
    .UseSwaggerGen();

await app.RunAsync();