namespace ProductAggregate.API.Infrastructure.Repositories
{
    public interface IServerRepository : IRepository<Server>
    {
    }

    public class ServerRepository : Repository<Server>, IServerRepository, ITransient
    {
        public ServerRepository(AppDbContext context) : base(context)
        {
        }
    }
}
