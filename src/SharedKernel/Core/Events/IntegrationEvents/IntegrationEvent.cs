namespace Core.IntegrationEvents.IntegrationEvents
{
    /// <summary>
    /// IntegrationEvent with default <see cref="Guid"/> Id
    /// </summary>
    public abstract class IntegrationEvent : IntegrationEvent<Guid> 
    {
        public IntegrationEvent() : base(Guid.NewGuid()) { }
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
