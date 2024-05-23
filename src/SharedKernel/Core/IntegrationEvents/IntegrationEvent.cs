namespace Core.IntegrationEvents
{
    public abstract class IntegrationEvent : IntegrationEvent<string> { }

    public abstract class IntegrationEvent<TKey>
    {
        protected IntegrationEvent() {}
        protected IntegrationEvent(TKey id, long version)
        {
            Id = id;
            Version = version;
        }
        public TKey Id { get; set; }
        public long Version { get; set; }
    }
}
