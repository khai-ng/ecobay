using Core.EntityFramework.ServiceDefault;
using Core.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Reflection;

namespace Core.EntityFramework.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly DbContext _dbContext;
        private IDbContextTransaction? _currentTransaction;

        public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;

        public UnitOfWork(IServiceProvider serviceProvider, IMediator mediator)
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;

            var baseDbContextTypes = Assembly.GetEntryAssembly()?.GetTypes().Where(x => x.IsSubclassOf(typeof(BaseDbContext))).First();
            _dbContext = (DbContext)_serviceProvider.GetRequiredService(baseDbContextTypes!);
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            var domainEntities = _dbContext.ChangeTracker.Entries<AggregateRoot>()
                .Where(x => x.Entity.Events != null && x.Entity.Events.Count != 0);

            if (domainEntities != null && domainEntities.Any())
            {
                var domainEvents = domainEntities.SelectMany(x => x.Entity.Events);
                foreach (var domainEvent in domainEvents)
                    await _mediator.Publish(domainEvent, ct);

                foreach (var item in domainEntities)
                    item.Entity.ClearEvents();
            }

            await _dbContext.SaveChangesAsync(ct);
        }

        protected async Task<IDbContextTransaction?> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;
            _currentTransaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            return _currentTransaction;
        }

        protected async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        protected void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
