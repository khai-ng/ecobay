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
using Core.Kafka;
using Core.Marten;
using Autofac.Core;
using Core.Events.EventStore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();

builder.AddServiceDefaults();
builder.AddAutofac();
builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
});

builder.Services.AddKafkaConsumer();

builder.Services.AddMarten(builder.Configuration);
builder.Services.AddScoped(typeof(IEventStoreRepository<,>), typeof(MartenRepository<>));

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