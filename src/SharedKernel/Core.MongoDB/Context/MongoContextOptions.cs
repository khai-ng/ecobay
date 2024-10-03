namespace Core.MongoDB.Context
{
    public class MongoContextOptions
    {
        public MongoConnectionOptions Connection { get; set; } = new();
        public MongoTelemetryOptions Telemetry { get; set; } = new();

    }

    public class MongoTelemetryOptions
    {
        public bool Enable { get; set; } = false;
        public bool CaptureCommandText { get; set; } = false;
    }
    public class MongoConnectionOptions
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;
    }
}
