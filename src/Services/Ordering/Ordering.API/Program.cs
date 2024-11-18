using Core.AspNet.Endpoints;
using Core.AspNet.Extensions;
using Core.Autofac;
using Core.EntityFramework;
using Core.Kafka;
using Core.Marten;
using Core.MediaR;
using FastEndpoints;
using FastEndpoints.Swagger;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using Ordering.API.Infrastructure;
using Ordering.API.Presentation.Extensions;
using System.Reflection;
using Core.AspNet.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(rb => rb.AddService("Ordering.API"));
builder.AddKafkaOpenTelemetry()
    .AddMartenOpenTelemetry()
    .AddEFCoreOpenTelemetry();
builder.AddAutofac();
builder.AddServiceDefaults();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
});
builder.Services.AddKafkaCompose();
builder.Services.AddMarten(builder.Configuration);
builder.Services.AddMartenRepository<Ordering.API.Domain.OrderAggregate.Order>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(opt =>
	{
        opt.AddKeyCloakConfigs(builder.Configuration);

    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
        await context.Database.MigrateAsync();
}

app.UseServiceDefaults();
app.UseHttpsRedirection();
app.UseDefaultSwaggerRedirection();
app.UseFastEndpoints(config => config.DefaultResponseConfigs())
    .UseSwaggerGen();

app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();