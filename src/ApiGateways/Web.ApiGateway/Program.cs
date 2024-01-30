using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure.Kernel.Dependency;
using ServiceDefaults;
using Web.ApiGateway.Configurations;
using Web.ApiGateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .AddSwaggerGen()
    .SwaggerDocument();

//builder.Services.AddEndpointsApiExplorer()
//    .AddSwaggerGen();

builder.AddServiceDefaults();
builder.AddAutofac();
builder.Services.AddAuthorization();
builder.Services.AddGrpcServices();
builder.Services.AddReverseProxy(builder.Configuration);

builder.Services.Configure<UrlsConfiguration>(builder.Configuration.GetSection("urls"));
var app = builder.Build();

app.UseServiceDefaults();
app.UseHttpsRedirection();

app.UseDefaultExceptionHandler();
app.UseFastEndpoints();

//app.UseSwaggerGen();

app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Web ApiGateway");
    opt.SwaggerEndpoint("/identity/swagger/v1/swagger.json", "Identity Api");
    opt.SwaggerEndpoint("/employee/swagger/v1/swagger.json", "Employee Api");
});

app.MapGetSwaggerForYarp(app.Configuration);
app.MapReverseProxy();

await app.RunAsync();