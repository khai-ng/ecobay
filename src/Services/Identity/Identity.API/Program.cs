using Autofac.Extensions.DependencyInjection;
using FastEndpoints;
using FastEndpoints.Swagger;
using Identity.API.Extension;
using Identity.Infrastructure;
using Identity.Infrastructure.Authentication;
using Kernel.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ServiceDefaults;
using SharedKernel.Kernel.Dependency;
using System.Net;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();

//builder.Services.AddEndpointsApiExplorer()
//.AddSwaggerGen();

builder.AddServiceDefaults();
builder.AddAutofac();
builder.Services.AddDbContexts(builder.Configuration);

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
        await context.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseServiceDefaults();

app.UseExceptionHandler(opt => { });
app.UseFastEndpoints(config =>
{
    var jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    config.Errors.ResponseBuilder = (failures, ctx, statusCode) =>
    {
        return new HttpErrorResult((HttpStatusCode)statusCode, 
            failures.Select(e => new ErrorDetail(e.PropertyName, e.ErrorMessage)));
    };

    config.Serializer.ResponseSerializer = (reposnse, dto, cType, jsonContext, ct) =>
    {
        reposnse.ContentType = cType;
        return reposnse.WriteAsync(JsonSerializer.Serialize(dto, jsonSerializerOptions), ct);
    };
})
    .UseSwaggerGen();

await app.RunAsync();