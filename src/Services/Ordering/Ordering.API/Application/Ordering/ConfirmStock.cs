using Core.Autofac;
using Core.Events.EventStore;
using Core.Kafka.Producers;
using Core.Result.AppResults;
using Core.SharedKernel;
using Marten.Events;
using MediatR;
using Ordering.API.Application.Common.Abstractions;
using Ordering.API.Application.IntegrationEvents;
using Ordering.API.Domain.OrderAgrregate;
using Ordering.API.Application.Dto.Order;

namespace Ordering.API.Application.Services
{
    public class ConfirmStock : IRequestHandler<ConfirmStockRequest, AppResult<string>>, ITransient
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventStoreRepository<Order> _eventStoreRepository;
        private readonly IKafkaProducer _kafkaProducer;

        public ConfirmStock(
            IOrderRepository orderRepository, 
            IUnitOfWork unitOfWork, 
            IEventStoreRepository<Order> eventStoreRepository, 
            IKafkaProducer kafkaProducer)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _eventStoreRepository = eventStoreRepository;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<AppResult<string>> Handle(ConfirmStockRequest request, CancellationToken ct)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId).ConfigureAwait(false);

            if (order == null)
                return AppResult.Invalid(new ErrorDetail($"Can not find order {request.OrderId}"));

            //order.SetStockConfirmed();
            //_orderRepository.Update(order);

            //await _eventStoreRepository.Update(order.Id, order, order.Version, ct: ct).ConfigureAwait(false);
            //await _unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);

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
