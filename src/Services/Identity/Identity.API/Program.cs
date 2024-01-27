using Autofac.Extensions.DependencyInjection;
using FastEndpoints;
using FastEndpoints.Swagger;
using Identity.Domain.Constants;
using Identity.Infrastructure;
using Identity.Infrastructure.Authentication;
using Infrastructure.Kernel.Dependency;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();

//builder.Services.AddEndpointsApiExplorer()
//.AddSwaggerGen();

builder.AddServiceDefaults();
builder.AddAutofac();

builder.Services.AddAuthorization();

builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

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

app.UseServiceDefaults();
app.UseHttpsRedirection();

app.UseDefaultExceptionHandler();
app.UseFastEndpoints()
    .UseSwaggerGen();

//app.UseSwagger();
//app.UseSwaggerUI();
//app.MapEndpoints();

await app.RunAsync();