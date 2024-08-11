namespace Core.Marten
{
    public class MartenConfig
    {
        public required string ConnectionString { get; set; }
    public required string WriteSchema { get; set;}
    public required string ReadSchema { get; set;}
    }
}
