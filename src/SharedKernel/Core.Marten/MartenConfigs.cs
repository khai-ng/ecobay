namespace Core.Marten
{
    public class MartenConfigs
    {
        public required string ConnectionString { get; set; }
        public required string WriteSchema { get; set; }
        public required string ReadSchema { get; set; }
        public bool EnableDaemon { get; set; } = false;
    }
}
