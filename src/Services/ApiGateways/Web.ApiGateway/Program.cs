var builder = WebApplication.CreateBuilder(args);

builder.AddAutofac()
    .AddServiceDefaults();

builder.Services
    .AddSwaggerGen(opt => opt.AddKeyCloakSecurity(builder.Configuration))
    .SwaggerDocument();

builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri("http://order-api/hc"), name: "orderingapi-check")
    .AddUrlGroup(new Uri("http://product-api-1/hc"), name: "productapi-check");

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
        //opt.SwaggerEndpoint("/orders/swagger/v1/swagger.json", "Order Api");
        //opt.SwaggerEndpoint("/products/swagger/v1/swagger.json", "Product Api");
    });
//app.MapGetSwaggerForYarp(app.Configuration);

app.MapReverseProxy();

await app.RunAsync();