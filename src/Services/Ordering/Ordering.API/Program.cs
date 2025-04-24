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
    .AddDbContext(builder.Configuration)
    .AddMediatRDefaults()
    .AddKafkaCompose()
    .AddMarten(builder.Configuration);

builder.Services.AddMartenRepository<Order>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
        await context.Database.MigrateAsync();
}

app.UseServiceDefaults()
    .UseFastEndpoints(config => config.DefaultResponseConfigs());

app.UseDefaultSwaggerRedirection()
    .UseSwaggerGen();

await app.RunAsync();