using Core.EntityFramework.ServiceDefault;
using Core.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.EntityFramework.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly DbContext _dbContext;

        public UnitOfWork(IServiceProvider serviceProvider, IMediator mediator)
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;

            var baseDbContextTypes = Assembly.GetEntryAssembly()?.GetTypes().Where(x => x.IsSubclassOf(typeof(BaseDbContext))).First();
            _dbContext = (DbContext)_serviceProvider.GetRequiredService(baseDbContextTypes!);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            var domainEntities = _dbContext.ChangeTracker.Entries<AggregateRoot>()
                .Where(x => x.Entity.Events != null && x.Entity.Events.Count != 0);

            if(domainEntities != null && domainEntities.Any())
            {
                var domainEvents = domainEntities.SelectMany(x => x.Entity.Events);
                foreach (var domainEvent in domainEvents)
                    await _mediator.Publish(domainEvent, ct);

                foreach (var item in domainEntities)
                    item.Entity.ClearEvents();
            }

            await _dbContext.SaveChangesAsync(ct);
        }
    }
}
