using Autofac.Extensions.DependencyInjection;
using FastEndpoints;
using FastEndpoints.Swagger;
using Identity.API.Extension;
using Identity.Application.Behaviours;
using Identity.Infrastructure;
using Identity.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ServiceDefaults;
using SharedKernel.Kernel.Dependency;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();

//builder.Services.AddEndpointsApiExplorer()
//.AddSwaggerGen();

builder.AddServiceDefaults();
builder.AddAutofac();
builder.Services.AddDbContexts(builder.Configuration);

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
});

Log.Logger = new LoggerConfiguration()
    .CreateBootstrapLogger();

builder.Host.UseSerilog((context, serviceProvider, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();
app.UseExceptionHandler(opt => { });

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
        await context.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseServiceDefaults();

app.UseFastEndpoints(config => config.CommonResponseConfigs())
    .UseSwaggerGen();

await app.RunAsync();