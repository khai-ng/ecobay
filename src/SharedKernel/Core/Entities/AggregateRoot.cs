using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Core.Events.DomainEvents;

namespace Core.SharedKernel
{
    public abstract class AggregateRoot<TKey>: Entity<TKey>
    {
        [NonSerialized]
        private readonly Queue<IDomainEvent<TKey>> _events = new();

        public long Version { get; protected set; }
        [JsonIgnore]
        public IReadOnlyCollection<IDomainEvent<TKey>> Events => _events.ToImmutableArray();


        protected AggregateRoot() { }
        protected AggregateRoot(TKey id) : base(id) { }

        public virtual void Apply(IDomainEvent<TKey> @event) { }

        public void Apply(object @event)
        {
            if (@event is IDomainEvent<TKey> typedEvent)
                Apply(typedEvent);
        }
        public void Enqueue(IDomainEvent<TKey> @event)
        {
            ArgumentNullException.ThrowIfNull(nameof(@event));

            _events.Enqueue(@event);
            Apply(@event);
            Version++;
        }

        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}
