using Core.AspNet.Endpoints;
using Core.AspNet.Extensions;
using Core.AspNet.Identity;
using Core.Autofac;
using Core.Kafka;
using Core.MediaR;
using Core.MongoDB;
using Core.MongoDB.Context;
using FastEndpoints;
using FastEndpoints.Swagger;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Bson.Serialization.Conventions;
using OpenTelemetry.Resources;
using ProductAggregate.API.Infrastructure;
using System.Reflection;
using Hangfire;
using ProductAggregate.API.Application.Common.Abstractions;
using ProductAggregate.API.Infrastructure.Repositories;
using ProductAggregate.API.Presentation.Configurations;

var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(rb => rb.AddService("ProductAggregate.API"));
builder.AddKafkaOpenTelemetry()
    .AddMongoTelemetry();
builder.AddServiceDefaults();
builder.AddAutofac();
builder.Services.AddMongoDbContext<AppDbContext>(options =>
{ 
    options.Connection = builder.Configuration.GetSection("Mongo:Connection").Get<MongoConnectionOptions>()!;
    options.Telemetry.Enable = true;
});
builder.Services.AddKafkaCompose();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
});
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(opt =>
	{
		opt.AddKeyCloakConfigs(builder.Configuration);
    });
builder.Services.AddTransient<IProductMigrationRepository, ProductMigrationRepository>();

if(builder.Environment.IsDevelopment())
{
    builder.Services.AddHangfireDefaults(builder.Configuration);
}

var app = builder.Build();

app.UseServiceDefaults();
app.UseHttpsRedirection();
app.UseDefaultSwaggerRedirection();
app.UseFastEndpoints(config => config.DefaultResponseConfigs())
    .UseSwaggerGen();

app.UseAuthentication();
app.UseAuthorization();

if(app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard();
    app.AddHangFireJob();
}

await app.RunAsync();