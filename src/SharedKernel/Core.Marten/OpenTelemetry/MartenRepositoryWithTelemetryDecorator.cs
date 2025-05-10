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
        public Task<TEntity?> FindAsync(Guid id, CancellationToken ct)
            => martenRepository.FindAsync(id, ct);

        public Task<long> AddAsync(Guid id, TEntity aggregate, CancellationToken ct = default)
        {
            using (var activity = MartenActivityScope.StartActivity(nameof(AddAsync)))
            {
                activity?.SetTag("eventsourcing.entity", nameof(TEntity));
                activity?.SetTag("eventsourcing.entity_id", id);
                activity?.SetTag("eventsourcing.entity_version", aggregate.Version);

                documentSession.PropagateTelemetry(activity);
                return martenRepository.AddAsync(id, aggregate, ct);
            }
        }

        public Task<long> UpdateAsync(Guid id, TEntity aggregate, long? expectedVersion = null, CancellationToken ct = default)
        {
            using (var activity = MartenActivityScope.StartActivity(nameof(UpdateAsync)))
            {
                activity?.SetTag("eventsourcing.entity", nameof(TEntity));
                activity?.SetTag("eventsourcing.entity_id", id);
                activity?.SetTag("eventsourcing.entity_version", aggregate.Version);

                documentSession.PropagateTelemetry(activity);
                return martenRepository.UpdateAsync(id, aggregate, expectedVersion, ct);
            }
        }
    }
}
