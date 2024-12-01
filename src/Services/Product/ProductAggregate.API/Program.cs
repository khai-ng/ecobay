var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddAutofac();

builder.Services.AddSwaggerGen().SwaggerDocument();

builder.Services.AddOpenTelemetry()
    .AddKafkaOpenTelemetry()
    .AddMongoTelemetry();

builder.Services.AddHealthChecks()
    .AddMongoDb(builder.Configuration.GetSection("Mongo:Connection:ConnectionString").ToString()!);

builder.Services
    .AddFastEndpoints()
    .AddMongoDbContext<AppDbContext>(options =>
    { 
        options.Connection = builder.Configuration.GetSection("Mongo:Connection").Get<MongoConnectionOptions>()!;
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