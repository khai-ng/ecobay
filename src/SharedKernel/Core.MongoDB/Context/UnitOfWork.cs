using Core.SharedKernel;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.MongoDB.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MongoContext _context;
        public UnitOfWork(IServiceProvider serviceProvider)
        {
            var baseDbContextTypes = Assembly.GetEntryAssembly()?
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(MongoContext)))
                .First();
            _context = (MongoContext)serviceProvider.GetRequiredService(baseDbContextTypes!);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
            => _context.SaveChangesAsync(ct);
    }
}
