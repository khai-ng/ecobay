using Core.Marten.Repository;
using Core.Mediator;
using Marten;

namespace Ordering.API.Infrastruture.Repositories
{
    public class OrderRepository : MartenRepository<Order>, IOrderRepository, ITransient
    {
        public OrderRepository(IDocumentSession documentSession, IMediator mediator) : base(documentSession, mediator)
        { }

        public async Task<IEnumerable<OrderView>> GetAsync(Guid? Id = null, Guid? BuyerId = null, int? StatusId = null)
        {
            return await _documentSession.Query<OrderView>()
                .Where(x => Id == null || x.Id == Id)
                .Where(x => BuyerId == null || x.BuyerId == BuyerId)
                .Where(x => StatusId == null || x.Status.Id == StatusId)
                .OrderByDescending(x => x.CreatedAtTicks)
                .ToListAsync(token: default)
                .ConfigureAwait(false);
        }
    }
}
