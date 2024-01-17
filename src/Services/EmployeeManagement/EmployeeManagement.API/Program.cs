using Autofac.Extensions.DependencyInjection;
using EmployeeManagement.Domain.Constants;
using EmployeeManagement.Infrastructure;
using FastEndpoints;
using FastEndpoints.Swagger;
using GrpcEmployee;
using Infrastructure.Kernel.Dependency;
using Microsoft.EntityFrameworkCore;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();

builder.AddServiceDefaults();
builder.AddAutofac();
builder.Services.AddGrpc();
builder.Services.AddAuthorization();

builder.Services.AddDbContextPool<AppDbContext>((service, opt) =>
{
    var connection = builder.Configuration.GetConnectionString(AppEnvironment.DB_SCHEMA);
    opt.UseSqlServer(connection, option => option.CommandTimeout(100));
});

var app = builder.Build();

app.MapGrpcService<EmployeeService>();

app.UseServiceDefaults();
app.UseHttpsRedirection();

app.UseDefaultExceptionHandler();
app.UseFastEndpoints()
    .UseSwaggerGen();

await app.RunAsync();