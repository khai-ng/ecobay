using Core.Mediator;
using System.Text.Json.Serialization;

namespace Core.IntegrationEvents.IntegrationEvents
{
    /// <summary>
    /// IntegrationEvent with default <see cref="Guid"/> Identity
    /// </summary>
    public abstract record IntegrationEvent : IntegrationEvent<Guid> 
    {
        public IntegrationEvent() : base(Guid.CreateVersion7()) { }
    }

    public abstract record IntegrationEvent<TKey> : IRequest
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
