using Core.Entities;
using Core.Repositories;
using Marten;

namespace Core.Marten.OpenTelemetry
{
    internal class MartenRepositoryWithTelemetryDecorator<TEntity>(
        IEventStoreRepository<TEntity> martenRepository,
        IDocumentSession documentSession) : IEventStoreRepository<TEntity>
            where TEntity : AggregateRoot<Guid>

    {
        public Task<TEntity?> Find(Guid id, CancellationToken ct)
            => martenRepository.Find(id, ct);

        public Task<long> Add(Guid id, TEntity aggregate, CancellationToken ct = default)
        {
            using (var activity = MartenActivityScope.StartActivity(nameof(Add)))
            {
                activity?.SetTag("eventsourcing.entity", nameof(TEntity));
                activity?.SetTag("eventsourcing.entity_id", id);
                activity?.SetTag("eventsourcing.entity_version", aggregate.Version);

                documentSession.PropagateTelemetry(activity);
                return martenRepository.Add(id, aggregate, ct);
            }
        }

        public Task<long> Update(Guid id, TEntity aggregate, long? expectedVersion = null, CancellationToken ct = default)
        {
            using (var activity = MartenActivityScope.StartActivity(nameof(Update)))
            {
                activity?.SetTag("eventsourcing.entity", nameof(TEntity));
                activity?.SetTag("eventsourcing.entity_id", id);
                activity?.SetTag("eventsourcing.entity_version", aggregate.Version);

                documentSession.PropagateTelemetry(activity);
                return martenRepository.Update(id, aggregate, expectedVersion, ct);
            }
        }
    }
}
