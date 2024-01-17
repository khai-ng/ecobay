using ServiceDefaults;
using Web.ApiGateway.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;
using Web.ApiGateway.Configurations;
using Infrastructure.Kernel.Dependency;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();

builder.AddServiceDefaults();
builder.AddAutofac();
builder.Services.AddGrpcServices();
builder.Services.AddAuthorization();
builder.Services.AddReverseProxy(builder.Configuration);

builder.Services.Configure<UrlsConfiguration>(builder.Configuration.GetSection("urls"));
var app = builder.Build();

app.UseServiceDefaults();
app.UseHttpsRedirection();

app.UseDefaultExceptionHandler();
app.UseFastEndpoints()
    .UseSwaggerGen();

app.MapReverseProxy();

await app.RunAsync();