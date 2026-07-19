using Core.Kafka.Consumers;
using Core.Kafka.Producers;
using JasperFx.Core;
using JasperFx.Events.Projections;
using Ordering.API.Infrastruture.Projections;

var builder = WebApplication.CreateBuilder(args);

builder.AddAutofac()
    .AddServiceDefaults();

builder.Services.AddSwaggerGen().SwaggerDocument();

builder.Services.AddHealthChecks()
    .AddMartenAsyncDaemonHealthCheck();

builder.Services.AddOpenTelemetry()
    .AddKafkaOpenTelemetry()
    .AddEFCoreOpenTelemetry()
    .AddMartenOpenTelemetry();

builder.Services
    .AddFastEndpoints()
    .AddDefaultMediator()
    .AddKafkaCompose(
        p => p = builder.Configuration.GetRequiredSection("Kafka:Producer").Get<KafkaProducerConfigs>()!,
        c => c = builder.Configuration.GetRequiredSection("Kafka:Consumer").Get<KafkaConsumerConfigs>()!
    )
    //read: https://martendb.io/events/projections/async-daemon.html
    .AddDefaultMarten(m => m = builder.Configuration.GetRequiredSection("EventStore").Get<MartenConfigs>()!,
        options =>
        {
            options.Schema.For<OrderView>()
            .Index(x => x.Id)
            .Index(x => x.BuyerId)
            .Index(x => x.Status.Id)
            .Index(x => x.CreatedAtTicks);

            options.Projections.Add<OrderViewProjection>(ProjectionLifecycle.Async);
            options.Projections.SlowPollingTime = 5.Seconds();
        });

builder.Services.AddMartenRepository<Order>();

var app = builder.Build();


app.UseServiceDefaults()
    .UseFastEndpoints(config => config.DefaultResponseConfigs());

app.UseDefaultSwaggerRedirection()
    .UseSwaggerGen();
await app.RunAsync();

////read: https://martendb.io/events/projections/async-daemon.html#using-the-async-daemon-from-documentstore
//using var daemon = await app.DocumentStore().BuildProjectionDaemonAsync();
//await daemon.StartAllAsync();
//await daemon.RebuildProjectionAsync<OrderProjection>(CancellationToken.None);
//await daemon.StopAllAsync();