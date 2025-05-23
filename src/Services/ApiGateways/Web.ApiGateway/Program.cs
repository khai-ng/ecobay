var builder = WebApplication.CreateBuilder(args);

builder.AddAutofac()
    .AddServiceDefaults();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3001")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
        });
});

builder.Services
    .AddSwaggerGen(opt => opt.AddKeyCloakSecurity(builder.Configuration["Keycloak:AuthorizationUrl"]!))
    .SwaggerDocument();

builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri("http://order-api/hc"), name: "orderingapi-check")
    .AddUrlGroup(new Uri("http://product-api-1/hc"), name: "productapi-1-check");

builder.Services
    .AddFastEndpoints()
    .AddReverseProxy(builder.Configuration);

var app = builder.Build();

app.UseCors();

app.UseServiceDefaults()   
    .UseFastEndpoints(config => config.DefaultResponseConfigs());

app.UseDefaultSwaggerRedirection()
    .UseSwagger()
    .UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Web ApiGateway");
        opt.SwaggerEndpoint("/order/swagger/v1/swagger.json", "Ordering Api");
        opt.SwaggerEndpoint("/product/swagger/v1/swagger.json", "Product Api");
    });
app.MapGetSwaggerForYarp(app.Configuration);

app.MapReverseProxy();

await app.RunAsync();