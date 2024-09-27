using Core.Autofac;
using Core.Kafka.Producers;
using Core.AppResults;
using MediatR;
using Ordering.API.Application.Common.Abstractions;
using Ordering.API.Application.Dto.Order;
using Ordering.API.Application.IntegrationEvents;

namespace Ordering.API.Application.Services
{
    public class ConfirmStock : IRequestHandler<ConfirmStockRequest, AppResult<string>>, ITransient
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IKafkaProducer _kafkaProducer;

        public ConfirmStock(
            IOrderRepository orderRepository,
            IKafkaProducer kafkaProducer)
        {
            _orderRepository = orderRepository;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<AppResult<string>> Handle(ConfirmStockRequest request, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId).ConfigureAwait(false);

            if (order == null)
                return AppResult.Invalid(new ErrorDetail($"Can not find order {request.OrderId}"));

            var orderConfirmStockEvent = 
                new OrderConfirmStockIntegrationEvent(
                    order.Id,
                    order.OrderItems.Select(x => new ProductUnit(x.ProductId, x.Unit))
                );

            _ = _kafkaProducer.PublishAsync(orderConfirmStockEvent, ct);

            return AppResult.Success("Successful");
        }
    }                                                                                           
}
