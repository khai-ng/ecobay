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
    var connection = builder.Configuration.GetConnectionString(AppEnvironment.DB_SCHEMA)!;
    //opt.UseSqlServer(connection, option => option.CommandTimeout(100));
    opt.UseMySQL(connection);
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        await context.Database.MigrateAsync();
    }
}

app.MapGrpcService<EmployeeService>();

app.UseServiceDefaults();
app.UseHttpsRedirection();

app.UseDefaultExceptionHandler();
app.UseFastEndpoints()
    .UseSwaggerGen();

await app.RunAsync();