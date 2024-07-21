using System.Collections.Immutable;

namespace Core.SharedKernel
{
    public abstract class AggregateRoot<TKey>: Entity<TKey>
    {

        private readonly Queue<IDomainEvent<TKey>> _events = new();

        public long Version { get; private set; }
        public IReadOnlyCollection<IDomainEvent<TKey>> Events => _events.ToImmutableArray();

        protected AggregateRoot(TKey id) : base(id)
        {
            Id = id;
        }

        public void AddEvent(IDomainEvent<TKey> @event)
        {
            ArgumentNullException.ThrowIfNull(nameof(@event));

            _events.Enqueue(@event);
            Version++;
        }

        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}
