using System.Text.Json.Serialization;

namespace Core.IntegrationEvents.IntegrationEvents
{
    /// <summary>
    /// IntegrationEvent with default <see cref="Guid"/> Identity
    /// </summary>
    public abstract record IntegrationEvent : IntegrationEvent<Guid> 
    {
        public IntegrationEvent() : base(Guid.NewGuid()) { }
    }

    public abstract record IntegrationEvent<TKey>
    {
        protected IntegrationEvent(TKey id)
        {
            Id = id;
            CreatedDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        protected IntegrationEvent(TKey id, DateTime createdDate)
        {
            Id = id;
            CreatedDate = createdDate;
        }
        public TKey Id { get; private set; }
        public DateTime CreatedDate { get; private set; }
    }
}
