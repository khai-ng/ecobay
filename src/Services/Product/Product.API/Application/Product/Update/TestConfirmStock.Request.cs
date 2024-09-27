using Core.AppResults;
using MediatR;

namespace Product.API.Application.Product.Update
{
    public class ConfirmStockRequest(string id, int units) : IRequest<AppResult>
    {
        public string Id { get; set; } = id;
        public int Units { get; set; } = units;
    }
}
