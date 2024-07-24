using Core.Autofac;
using FastEndpoints;
using FastEndpoints.Swagger;
using Core.AspNet.Extensions;
using Ordering.API.Presentation.Extensions;
using System.Reflection;
using Core.MediaR;
using MediatR;
using Ordering.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();

//builder.Services.AddEndpointsApiExplorer()
//.AddSwaggerGen();

builder.AddServiceDefaults();
builder.AddAutofac();
builder.Services.AddDbContexts(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
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
app.UseDefaultSwaggerRedirection();

app.UseFastEndpoints(config => config.CommonResponseConfigs())
    .UseSwaggerGen();

await app.RunAsync();