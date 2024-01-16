using ServiceDefaults;
using Web.ApiGateway.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddFastEndpoints()
//    .AddSwaggerGen()
//    .SwaggerDocument();

builder.AddServiceDefaults();
builder.Services.AddAuthorization();
builder.Services.AddReverseProxy(builder.Configuration);

var app = builder.Build();

app.UseServiceDefaults();
app.UseHttpsRedirection();

app.UseDefaultExceptionHandler();
//app.UseFastEndpoints()
//    .UseSwaggerGen();

app.MapReverseProxy();

await app.RunAsync();