using Core.AspNet.Endpoints;
using Core.AspNet.Extensions;
using Core.Autofac;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OpenTelemetry.Resources;
using Web.ApiGateway.Extensions;
using Core.AspNet.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.AddAutofac()
    .AddServiceDefaults();

builder.Services
    .AddSwaggerGen(opt => opt.AddKeyCloakSecurity(builder.Configuration["Keycloak:AuthorizationUrl"]!))
    .SwaggerDocument();

builder.Services  
    .AddOpenTelemetry()
    .ConfigureResource(rb => rb.AddService("Web.ApiGateway"));

builder.Services
    .AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

builder.Services
    .AddFastEndpoints()
    .AddReverseProxy(builder.Configuration)
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.AddKeyCloakConfigs(builder.Configuration);
    });

var app = builder.Build();

app.UseServiceDefaults()
    .UseHttpsRedirection()
    .UseFastEndpoints(config => config.DefaultResponseConfigs());

app.UseDefaultSwaggerRedirection()
    .UseSwagger()
    .UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Web ApiGateway");
        opt.SwaggerEndpoint("/ordering/swagger/v1/swagger.json", "Ordering Api");
        opt.SwaggerEndpoint("/product/swagger/v1/swagger.json", "Product Api");
    });
app.MapGetSwaggerForYarp(app.Configuration);

app.MapReverseProxy();

//app.MapHealthChecks("/hc", new HealthCheckOptions()
//{
//    Predicate = _ => true,
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});
app.MapHealthChecks("/hc");
app.MapHealthChecks("/liveness", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});

app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();