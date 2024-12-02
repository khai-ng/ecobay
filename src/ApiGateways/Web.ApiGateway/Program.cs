var builder = WebApplication.CreateBuilder(args);

builder.AddAutofac()
    .AddServiceDefaults();

builder.Services
    .AddSwaggerGen(opt => opt.AddKeyCloakSecurity(builder.Configuration["Keycloak:AuthorizationUrl"]!))
    .SwaggerDocument();

builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri("http://ordering-api/hc"), name: "orderingapi-check")
    .AddUrlGroup(new Uri("http://productaggregate-api/hc"), name: "productapi-check");

builder.Services
    .AddFastEndpoints()
    .AddReverseProxy(builder.Configuration);

var app = builder.Build();

app.UseServiceDefaults()   
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

await app.RunAsync();