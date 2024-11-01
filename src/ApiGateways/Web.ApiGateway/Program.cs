using Core.AspNet.Extensions;
using Core.Autofac;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OpenTelemetry.Resources;
using Web.ApiGateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen(opt => opt.AddKeyCloakSecurity(builder.Configuration["Keycloak:AuthorizationUrl"]!))
    .SwaggerDocument();
builder.AddServiceDefaults();
builder.AddAutofac();

builder.Services.AddReverseProxy(builder.Configuration);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.RequireHttpsMetadata = false;
        opt.Audience = builder.Configuration["Authentication:Audience"];
        opt.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]!;
        opt.TokenValidationParameters = new()
        {
            ValidIssuer = builder.Configuration["Authentication:ValidateIssuer"]
        };
    });

builder.Services.AddOpenTelemetry()
    .ConfigureResource(rb => rb.AddService("Web.ApiGateway"));

var app = builder.Build();

app.UseServiceDefaults();
app.UseHttpsRedirection();
app.UseDefaultSwaggerRedirection();
app.UseFastEndpoints(config => config.CommonResponseConfigs());

app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Web ApiGateway");
    opt.SwaggerEndpoint("/ordering/swagger/v1/swagger.json", "Ordering Api");
    opt.SwaggerEndpoint("/product/swagger/v1/swagger.json", "Product Api");
});

app.MapGetSwaggerForYarp(app.Configuration);
app.MapReverseProxy();

app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();