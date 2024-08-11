namespace Core.SharedKernel
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
