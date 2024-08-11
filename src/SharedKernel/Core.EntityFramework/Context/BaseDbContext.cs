using Core.EntityFramework.Identity;
using Core.EntityFramework.ServiceDefault;
using Core.SharedKernel;
using EFCore.BulkExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.EntityFramework.Context
{
    /// <summary>
    /// Abstracted <see cref="Ulid"/> converter and implemented <see cref="IUnitOfWork"/> 
    /// </summary>
    public abstract class BaseDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;
        public BaseDbContext() { }

        public BaseDbContext(DbContextOptions options) : base(options) { }

        public BaseDbContext(DbContextOptions options, IMediator mediator) : base(options) 
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public new async Task SaveChangesAsync(CancellationToken ct = default)
        {
            var a = await this.SaveChangesAsync(cancellationToken: ct);

            var domainEntities = ChangeTracker.Entries<AggregateRoot>()
                .Where(x => x.Entity.Events != null && x.Entity.Events.Count != 0)
                .ToList();
            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Events);

            domainEntities.ForEach(x => x.Entity.ClearEvents());

            foreach (var domainEvent in domainEvents)
                await _mediator.Publish(domainEvent, ct);

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>()
            //.HaveConversion<UlidToBytesConverter>()
            ;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
