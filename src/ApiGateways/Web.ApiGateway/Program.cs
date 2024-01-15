using ServiceDefaults;
using Web.ApiGateway.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.AddServiceDefaults();
builder.Services.AddReverseProxy(builder.Configuration);

var app = builder.Build();

app.UseServiceDefaults();
app.UseHttpsRedirection();

app.MapReverseProxy();
await app.RunAsync();