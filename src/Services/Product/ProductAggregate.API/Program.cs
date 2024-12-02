var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddAutofac();

builder.Services.AddSwaggerGen().SwaggerDocument();

builder.Services.AddOpenTelemetry()
    .AddKafkaOpenTelemetry()
    .AddMongoTelemetry();

var mongoConfig = builder.Configuration.GetSection("Mongo:Connection").Get<MongoConnectionOptions>()!;
builder.Services
    .AddHealthChecks()
    .AddMongoDb(mongoConfig.ConnectionString, name: mongoConfig.DatabaseName);

builder.Services
    .AddFastEndpoints()
    .AddMongoDbContext<AppDbContext>(options =>
    { 
        options.Connection = mongoConfig;
        options.Telemetry.Enable = true;
    })
    .AddKafkaCompose()
    .AddMediatRDefaults();

builder.Services.AddTransient<IProductMigrationRepository, ProductMigrationRepository>();

if(builder.Environment.IsDevelopment())
{
    builder.Services.AddHangfireDefaults(builder.Configuration);
}

var app = builder.Build();

app.UseServiceDefaults()
    .UseFastEndpoints(config => config.DefaultResponseConfigs());

app.UseDefaultSwaggerRedirection()  
    .UseSwaggerGen();

if(app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard();
    app.AddHangFireJob();
}

await app.RunAsync();