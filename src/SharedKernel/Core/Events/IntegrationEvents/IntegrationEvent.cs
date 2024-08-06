namespace Core.IntegrationEvents.IntegrationEvents
{
    /// <summary>
    /// IntegrationEvent with default <see cref="Ulid"/> Id
    /// </summary>
    public abstract class IntegrationEvent : IntegrationEvent<Ulid> 
    {
        public IntegrationEvent() : base(Ulid.NewUlid()) { }
    }

    public abstract class IntegrationEvent<TKey>
    {
        protected IntegrationEvent(TKey id)
        {
            Id = id;
            CreatedDate = DateTime.UtcNow;
        }
        public TKey Id { get; set; }
        public DateTime CreatedDate { get; private set; }
    }
}
