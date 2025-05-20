using Core.Entities;
using Core.Repositories;
using Marten;
using MediatR;
using System.Linq;

namespace Core.Marten.Repository
{
    public class MartenRepository<TEntity> : IEventStoreRepository<TEntity>
        where TEntity : AggregateRoot<Guid>
    {
        private readonly IDocumentSession _documentSession;
        private readonly IMediator _mediator;

        public MartenRepository(IDocumentSession documentSession, IMediator mediator)
        {
            _documentSession = documentSession;
            _mediator = mediator;
        }

        public async Task<TEntity?> FindAsync(Guid id, CancellationToken ct = default)
        {
            return await _documentSession.Events.AggregateStreamAsync<TEntity>(id, token: ct).ConfigureAwait(false);
        }

        public async Task<IEnumerable<EventEntity>> GetEventsAsync(Guid id, CancellationToken ct = default)
        {
            var events = await _documentSession.Events.FetchStreamAsync(id, token: ct).ConfigureAwait(false);
            return events.Select(x => new EventEntity(x.Id, x.Data, x.EventType, x.EventTypeName, x.Sequence, x.Version, x.Timestamp));
		}

        public async Task<long> AddAsync(Guid id, TEntity aggregate, CancellationToken ct = default)
        {
            _documentSession.Events.StartStream<TEntity>(id, aggregate.Events);

			foreach (var domainEvent in aggregate.Events)
				await _mediator.Publish(domainEvent, ct);
			
            aggregate.ClearEvents();

            await _documentSession.SaveChangesAsync(ct).ConfigureAwait(false);
            return aggregate.Events.Count;
        }

        public async Task<long> UpdateAsync(Guid id, TEntity aggregate, long? expectedVersion = null, CancellationToken ct = default)
        {
            var nextVersion = expectedVersion ?? aggregate.Version;
            _documentSession.Events.Append(id, nextVersion, aggregate.Events);

			foreach (var domainEvent in aggregate.Events)
				await _mediator.Publish(domainEvent, ct);

            aggregate.ClearEvents();
			
            await _documentSession.SaveChangesAsync(ct).ConfigureAwait(false);
            return aggregate.Events.Count;
        }
    }
}
