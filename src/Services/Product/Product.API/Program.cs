var camelCaseConventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddAutofac();

builder.Services.AddOpenTelemetry()
    .AddMongoTelemetry();

builder.Services
    .AddHealthChecks()
    .AddMongoDb(builder.Configuration.GetSection("Mongo:Connection:ConnectionString").ToString()!);

builder.Services
    .AddMongoDbContext<AppDbContext>(options =>
    {
        options.Connection = builder.Configuration.GetSection("Mongo:Connection").Get<MongoConnectionOptions>()!;
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