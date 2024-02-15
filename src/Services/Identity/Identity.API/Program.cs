using Autofac.Extensions.DependencyInjection;
using FastEndpoints;
using FastEndpoints.Swagger;
using Identity.API.Extension;
using Identity.Infrastructure;
using Identity.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ServiceDefaults;
using SharedKernel.Kernel.Dependency;
using System.Reflection;
using Identity.Application.Behaviours;
using Serilog.Templates;
using Serilog.Sinks.File;

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
    //.WriteTo.Console(new ExpressionTemplate("{ {ts: @t, msg: @m, lv: @l, ex: @x, ..@p} }\n"))
    .CreateLogger();

builder.Host.UseSerilog((hostContext, logCfg) =>
{
    logCfg.ReadFrom.Configuration(hostContext.Configuration);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
        await context.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseServiceDefaults();

app.UseExceptionHandler(opt => { });
app.UseFastEndpoints(config => config.CommonResponseConfigs())
    .UseSwaggerGen();

await app.RunAsync();