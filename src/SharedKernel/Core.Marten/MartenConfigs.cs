namespace Core.Marten
{
    public class MartenConfigs
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string WriteSchema { get; set; } = string.Empty;
        public string ReadSchema { get; set; } = string.Empty;
        public bool EnableDaemon { get; set; } = false;
    }
}
