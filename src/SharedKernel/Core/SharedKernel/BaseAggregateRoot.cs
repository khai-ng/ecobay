using System.Collections.Immutable;

namespace Core.SharedKernel
{
    public abstract class BaseAggregateRoot<TKey>
    {
        private readonly Queue<DomainEvent<TKey>> _events = new();

        protected BaseAggregateRoot() { }
        protected BaseAggregateRoot(TKey id)
        {
            Id = id;
        }

        public void AddEvent(DomainEvent<TKey> @event)
        {
            ArgumentNullException.ThrowIfNull(nameof(@event));

            _events.Enqueue(@event);
            Version++;
        }
        public TKey Id { get; set; }

        public long Version {get; private set; }

        public IReadOnlyCollection<DomainEvent<TKey>> Events => _events.ToImmutableArray();

        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}
