namespace Core.Entities
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
