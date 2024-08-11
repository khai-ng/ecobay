using Core.Events.EventStore;
using Core.SharedKernel;
using Marten;

namespace Core.Marten
{
    public class MartenRepository<TEntity> : IEventStoreRepository<TEntity, Guid>
        where TEntity : AggregateRoot<Guid>
    {
        private readonly IDocumentSession _documentSession;

        public MartenRepository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public async Task<TEntity?> Find(Guid id, CancellationToken ct)
        {
            return await _documentSession.Events.AggregateStreamAsync<TEntity>(id, token: ct);
        }

        public async Task<long> Add(Guid id, TEntity aggregate, CancellationToken ct = default)
        {
            _documentSession.Events.StartStream<TEntity>(id, aggregate.Events);
            await _documentSession.SaveChangesAsync(ct).ConfigureAwait(false);
            aggregate.ClearEvents();

            return aggregate.Events.Count;
        }

        public async Task<long> Update(Guid id, TEntity aggregate, long? expectedVersion = null, CancellationToken ct = default)
        {
            var nextVersion = (expectedVersion ?? aggregate.Version) + aggregate.Events.Count;
            _documentSession.Events.Append(id, nextVersion, aggregate.Events);
            await _documentSession.SaveChangesAsync(ct).ConfigureAwait(false);
            aggregate.ClearEvents();

            return aggregate.Events.Count;
        }
    }
}
