namespace Core.Marten
{
    public class MartenConnection
    {
        public required string ConnectionString { get; set; }
        public required string WriteSchema { get; set; }
        public required string ReadSchema { get; set; }
    }
}
