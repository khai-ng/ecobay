using Core.Autofac;
using Core.Result.AppResults;
using MediatR;
using Product.API.Application.Abstractions;
using Product.API.Domain.ServerAggregate;

namespace Product.API.Application.Servers
{
    public class GetServerHandler : IRequestHandler<GetServerRequest, AppResult<IEnumerable<Server>>>, IScoped
    {
        private readonly IServerRepository _serverRepository;

        public GetServerHandler(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }

        public async Task<AppResult<IEnumerable<Server>>> Handle(GetServerRequest request, CancellationToken cancellationToken)
        {
            var rs = await _serverRepository.GetAllAsync();

            return AppResult.Success(rs);
        }
    }
}
