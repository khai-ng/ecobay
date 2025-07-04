using Core.Entities;

namespace Core.MongoDB.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MongoContext _context;
        public UnitOfWork(MongoContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }
    }
}
