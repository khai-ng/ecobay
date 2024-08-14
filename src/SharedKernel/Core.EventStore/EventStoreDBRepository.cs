using Core.Events.DomainEvents;
using Core.Events.EventStore;
using Core.SharedKernel;
using EventStore.Client;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Core.Marten
{
    public class EventStoreDBRepository<TEntity> : IEventStoreRepository<TEntity>
        where TEntity : AggregateRoot<Guid>
    {
        private readonly EventStoreClient _eventStoreClient;

        public EventStoreDBRepository(EventStoreClient eventStoreClient)
        {
            _eventStoreClient = eventStoreClient;
        }

        public async Task<TEntity?> Find(Guid id, CancellationToken ct)
        {
            var readResult = _eventStoreClient.ReadStreamAsync(
                Direction.Forwards,
                id.ToString(),
                StreamPosition.Start,
                cancellationToken: ct);

             if (await readResult.ReadState.ConfigureAwait(false) == ReadState.StreamNotFound)
                return null;

            var aggregate = (TEntity)Activator.CreateInstance(typeof(TEntity), true)!;

            var domainEventTypes = Assembly.GetEntryAssembly()?.GetTypes().Where(x => x.IsAssignableFrom(typeof(IDomainEvent<>)));          
            await foreach (var @event in readResult)
            {
                var eventType = domainEventTypes?.First(x => x.Name == @event.Event.EventType);
                ArgumentNullException.ThrowIfNull(eventType);

                var eventData = JsonSerializer.Deserialize(
                    Encoding.UTF8.GetString(@event.Event.Data.Span),
                eventType);

                aggregate.Apply(eventData!);
            }

            return aggregate;
        }

        public async Task<long> Add(Guid id, TEntity aggregate, CancellationToken ct = default)
        {

            var result =  await _eventStoreClient.AppendToStreamAsync(
                id.ToString(),
                StreamState.NoStream,
                GetEventsToStore(aggregate),
                cancellationToken: ct);

            aggregate.ClearEvents();

            return aggregate.Events.Count;
        }

        public async Task<long> Update(Guid id, TEntity aggregate, long? expectedVersion = null, CancellationToken ct = default)
        {
            var eventsToAppend = GetEventsToStore(aggregate);
            //var nextVersion = expectedRevision ?? (ulong)(aggregate.Version - eventsToAppend.Count);


            aggregate.ClearEvents();

            return aggregate.Events.Count;
        }

        private static List<EventData> GetEventsToStore(TEntity aggregate)
        {
            var listEventData = aggregate.Events.Select(e =>
                new EventData(
                    Uuid.NewUuid(),
                    e.GetType().Name,
                    Encoding.UTF8.GetBytes(JsonSerializer.Serialize(e))
                ));
            return listEventData.ToList();
        }
    }
}
