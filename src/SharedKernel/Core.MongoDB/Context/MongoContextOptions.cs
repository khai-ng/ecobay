namespace Core.MongoDB.Context
{
    public class MongoContextOptions
    {
        public string ConnectionString { get; set; } = null!;
        public MongoTelemetryOptions Telemetry { get; set; } = new();

    }

    public class MongoTelemetryOptions
    {
        public bool Enable { get; set; } = false;
        public bool CaptureCommandText { get; set; } = false;
    }
}
