using Core.EntityFramework.OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

builder.AddAutofac()
    .AddServiceDefaults();

builder.Services.AddSwaggerGen().SwaggerDocument();

builder.Services.AddHealthChecks()
    .AddMySql(builder.Configuration.GetConnectionString("Default")!)
    .AddMartenAsyncDaemonHealthCheck();

builder.Services.AddOpenTelemetry()
    .AddKafkaOpenTelemetry()
    .AddEFCoreOpenTelemetry()
    .AddMartenOpenTelemetry();

builder.Services
    .AddFastEndpoints()
    .AddMediatRDefaults()
    .AddKafkaCompose()
    .AddMarten(builder.Configuration);

builder.Services.AddMartenRepository<Order>();

var app = builder.Build();

app.UseServiceDefaults()
    .UseFastEndpoints(config => config.DefaultResponseConfigs());

app.UseDefaultSwaggerRedirection()
    .UseSwaggerGen();

await app.RunAsync();