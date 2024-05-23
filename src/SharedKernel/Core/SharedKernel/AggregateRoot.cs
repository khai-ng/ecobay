using System.Collections.Immutable;

namespace Core.SharedKernel
{

    public abstract class AggregateRoot<TKey>: BaseEntity<TKey>
    {
        private readonly Queue<IDomainEvent<TKey>> _events = new();

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
        public TKey Id { get; set; }

        public long Version {get; private set; }

        public IReadOnlyCollection<IDomainEvent<TKey>> Events => _events.ToImmutableArray();

        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}
