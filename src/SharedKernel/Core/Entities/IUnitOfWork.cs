namespace Core.Entities
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> SaveChangesAsync(CancellationToken ct = default);
    }
}
