﻿using Core.Entities;
using Core.Repositories;
using Marten;

namespace Core.Marten.Repository
{
    public class MartenRepository<TEntity> : IEventStoreRepository<TEntity>
        where TEntity : AggregateRoot<Guid>
    {
        private readonly IDocumentSession _documentSession;

        public MartenRepository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;

        }

        public async Task<TEntity?> Find(Guid id, CancellationToken ct)
        {
            return await _documentSession.Events.AggregateStreamAsync<TEntity>(id, token: ct).ConfigureAwait(false);
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
            var nextVersion = expectedVersion ?? aggregate.Version;
            _documentSession.Events.Append(id, nextVersion, aggregate.Events);
            await _documentSession.SaveChangesAsync(ct).ConfigureAwait(false);
            aggregate.ClearEvents();

            return aggregate.Events.Count;
        }
    }
}
