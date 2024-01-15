using Autofac.Extensions.DependencyInjection;
using EmployeeManagement.Domain.Constants;
using EmployeeManagement.Infrastructure;
using FastEndpoints;
using Infrastructure.Kernel.Dependency;
using Microsoft.EntityFrameworkCore;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddAutofac();

builder.Services.AddFastEndpoints();
builder.Services.AddDbContextPool<AppDbContext>((service, opt) =>
{
    var connection = builder.Configuration.GetConnectionString(AppEnvironment.DB_SCHEMA);
    opt.UseSqlServer(connection, option => option.CommandTimeout(100));
});

var app = builder.Build();

app.UseServiceDefaults();
app.UseHttpsRedirection();

app.UseDefaultExceptionHandler();
app.UseFastEndpoints();

await app.RunAsync();