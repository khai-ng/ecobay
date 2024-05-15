using Autofac.Extensions.DependencyInjection;
using EmployeeMgt.API.Extensions;
using EmployeeMgt.Infrastructure;
using FastEndpoints;
using FastEndpoints.Swagger;
using GrpcEmployee;
using Microsoft.EntityFrameworkCore;
using Core.AspNet.Extensions;
using Core.Autofac;
using System.Reflection;
using Core.MediaR;
using MediatR;
using Core.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();

builder.AddServiceDefaults();
builder.AddAutofac();
builder.Services.AddDbContexts(builder.Configuration);

builder.Services.AddGrpc();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
});

//builder.Services.AddKafkaProducer();
//builder.Services.AddKafkaConsumer();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
        await context.Database.MigrateAsync();
}

app.MapGrpcService<EmployeeService>();
app.UseHttpsRedirection();
app.UseServiceDefaults();

app.UseFastEndpoints(config => config.CommonResponseConfigs())
    .UseSwaggerGen();

await app.RunAsync();