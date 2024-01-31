using Autofac.Extensions.DependencyInjection;
using EmployeeManagement.API.Extensions;
using EmployeeManagement.Infrastructure;
using FastEndpoints;
using FastEndpoints.Swagger;
using GrpcEmployee;
using SharedKernel.Kernel.Dependency;
using Microsoft.EntityFrameworkCore;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();

builder.AddServiceDefaults();
builder.AddAutofac();
builder.Services.AddDbContexts(builder.Configuration);

builder.Services.AddGrpc();
builder.Services.AddAuthorization();

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

app.UseDefaultExceptionHandler();
app.UseFastEndpoints()
    .UseSwaggerGen();

await app.RunAsync();