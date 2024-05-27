using Core.Result.AppResults;
using MediatR;
using Product.API.Domain.ServerAggregate;

namespace Product.API.Application.Servers
{
    public class GetServerRequest: IRequest<AppResult<IEnumerable<Server>>>
    {
    }
}
