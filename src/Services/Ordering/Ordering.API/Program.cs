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
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.AddAutofac()
    .AddServiceDefaults();

builder.Services
    .AddSwaggerGen()
    .SwaggerDocument();

builder.Services
    .AddKafkaOpenTelemetry()
    .AddEFCoreOpenTelemetry()
    .AddMartenOpenTelemetry()
    .AddOpenTelemetry()
    .ConfigureResource(rb => rb.AddService("Ordering.API"));

builder.Services
    .AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

builder.Services
    .AddFastEndpoints()
    .AddDbContext(builder.Configuration)
    .AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
    })
    .AddKafkaCompose()
    .AddMarten(builder.Configuration)
    .AddMartenRepository<Ordering.API.Domain.OrderAggregate.Order>();

builder.Services
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

app.UseServiceDefaults()
    .UseHttpsRedirection()
    .UseFastEndpoints(config => config.DefaultResponseConfigs());

app.UseDefaultSwaggerRedirection()
    .UseSwaggerGen();

app.MapHealthChecks("/hc");
app.MapHealthChecks("/liveness", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});

app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();