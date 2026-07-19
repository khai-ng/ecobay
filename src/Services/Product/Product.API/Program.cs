using Core.Kafka.Consumers;
using Core.Kafka.Producers;

var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddAutofac();

builder.Services.AddSwaggerGen().SwaggerDocument();

builder.Services.AddOpenTelemetry()
    .AddKafkaOpenTelemetry()
    .AddMongoTelemetry();

var mongoConfig = builder.Configuration.GetSection("Mongo").Get<MongoContextOptions>()!;
builder.Services
    .AddHealthChecks();
    //.AddMongoDb(s => new MongoClient(mongoConfig.ConnectionString));

builder.Services
    .AddFastEndpoints()
    .AddMongoDbContext<AppDbContext>(options =>
    { 
        options.ConnectionString = mongoConfig.ConnectionString;
        options.Telemetry.Enable = mongoConfig.Telemetry.Enable;
    })
    .AddKafkaCompose(
        p => p = builder.Configuration.GetRequiredSection("Kafka:Producer").Get<KafkaProducerConfigs>()!,
        c => c = builder.Configuration.GetRequiredSection("Kafka:Consumer").Get<KafkaConsumerConfigs>()!
    )
    .AddDefaultMediator();

//if(builder.Environment.IsDevelopment())
//{
//    builder.Services.AddHangfireDefaults(builder.Configuration);
//}

var app = builder.Build();

app.UseServiceDefaults()
    .UseFastEndpoints(config => config.DefaultResponseConfigs());

app.UseDefaultSwaggerRedirection()  
    .UseSwaggerGen();

//if(app.Environment.IsDevelopment())
//{
//    app.UseHangfireDashboard();
//    app.AddHangFireJob();
//}

await app.RunAsync();