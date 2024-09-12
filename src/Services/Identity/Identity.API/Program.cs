using Autofac.Extensions.DependencyInjection;
using Core.AspNet.Extensions;
using Core.Autofac;
using Core.Kafka;
using Core.MediaR;
using FastEndpoints;
using FastEndpoints.Swagger;
using Identity.API.Presentation.Extensions;
using Identity.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();

builder.AddKafkaOpenTelemetry();
builder.AddServiceDefaults();
builder.AddAutofac();

builder.Services.AddDbContext(builder.Configuration);
//builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
//builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
});

builder.Services.AddKafkaProducer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
        await context.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseServiceDefaults();
app.UseDefaultSwaggerRedirection();

app.UseFastEndpoints(config => config.CommonResponseConfigs())
    .UseSwaggerGen();

await app.RunAsync();