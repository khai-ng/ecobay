namespace Core.IntegrationEvents
{
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

    public abstract class IntegrationEvent : IntegrationEvent<Ulid>
    {
        protected IntegrationEvent() { }
        protected IntegrationEvent(long version)
        {
            Id = Ulid.NewUlid();
            Version = version;
        }

        protected IntegrationEvent(Ulid id, long version) : base(id, version)
        { }
    }
}
