var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddAutofac();

builder.Services.AddOpenTelemetry()
    .AddMongoTelemetry();

var mongoConfig = builder.Configuration.GetSection("Mongo:Connection").Get<MongoConnectionOptions>()!;
builder.Services
    .AddHealthChecks()
    .AddMongoDb(mongoConfig.ConnectionString, name: mongoConfig.DatabaseName);

builder.Services
    .AddMongoDbContext<AppDbContext>(options =>
    {
        options.Connection = mongoConfig;
        options.Telemetry.Enable = true;
    })
    .AddMediatRDefaults()
    .AddGrpc();

var app = builder.Build();

app.UseServiceDefaults();

app.MapGrpcService<GetProduct>();
app.MapGrpcService<UpdateProduct>();
//app.MapGrpcHealthChecksService();

await app.RunAsync();