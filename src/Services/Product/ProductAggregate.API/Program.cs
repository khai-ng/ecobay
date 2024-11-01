using Core.AspNet.Extensions;
using Core.Autofac;
using Core.Kafka;
using Core.MediaR;
using Core.MongoDB;
using Core.MongoDB.Context;
using FastEndpoints;
using FastEndpoints.Swagger;
using MediatR;
using MongoDB.Bson.Serialization.Conventions;
using ProductAggregate.API.Infrastructure;
using System.Reflection;
using Hangfire;
using ProductAggregate.API.Application.Common.Abstractions;
using ProductAggregate.API.Infrastructure.Repositories;
using ProductAggregate.API.Presentation.Configurations;
using OpenTelemetry.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(opt =>
	{
		opt.RequireHttpsMetadata = false;
		opt.Audience = builder.Configuration["Authentication:Audience"];
		opt.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]!;
		opt.TokenValidationParameters = new()
		{
			ValidIssuer = builder.Configuration["Authentication:ValidateIssuer"]
		};
	});
builder.Services.AddOpenTelemetry()
	.ConfigureResource(rb => rb.AddService("ProductAggregate.API"));

//builder.Services.AddTransient<IProductMigrationRepository, ProductMigrationRepository>();
//builder.Services.AddHangfireDefaults(builder.Configuration);

var app = builder.Build();

app.UseServiceDefaults();
app.UseHttpsRedirection();
app.UseDefaultSwaggerRedirection();
app.UseFastEndpoints(config => config.CommonResponseConfigs())
    .UseSwaggerGen();

app.UseAuthentication();
app.UseAuthorization();

//app.UseHangfireDashboard();
//app.AddHangFireJob();

await app.RunAsync();