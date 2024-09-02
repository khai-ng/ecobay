using Core.AspNet.Extensions;
using Core.Autofac;
using Core.Events.EventStore;
using Core.Kafka;
using Core.Marten;
using Core.MediaR;
using FastEndpoints;
using FastEndpoints.Swagger;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Infrastructure;
using Ordering.API.Presentation.Extensions;
using System.Reflection;

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

builder.Services.AddKafkaCompose();

builder.Services.AddMarten(builder.Configuration);
builder.Services.AddScoped(typeof(IEventStoreRepository<>), typeof(MartenRepository<>));

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