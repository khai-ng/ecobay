using Autofac.Extensions.DependencyInjection;
using FastEndpoints;
using Identity.Domain.Constants;
using Identity.Infrastructure;
using Identity.Infrastructure.Authentication;
using Infrastructure.Kernel.Dependency;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.AddServiceDefaults();
builder.AddAutofac();

builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

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
