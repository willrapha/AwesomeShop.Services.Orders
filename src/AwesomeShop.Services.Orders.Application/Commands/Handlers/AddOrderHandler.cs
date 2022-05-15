using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.MessageBus;
using AwesomeShop.Services.Orders.Infrastructure.ServiceDiscovery;
using MediatR;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AwesomeShop.Services.Orders.Application.Commands.Handlers
{
    public class AddOrderHandler : IRequestHandler<AddOrder, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBusClient _messageBus;
        private readonly IServiceDiscoveryService _serviceDiscoveryService;

        public AddOrderHandler(
            IOrderRepository orderRepository, 
            IMessageBusClient messageBus, 
            IServiceDiscoveryService serviceDiscoveryService)
        {
            _orderRepository = orderRepository;
            _messageBus = messageBus;
            _serviceDiscoveryService = serviceDiscoveryService;
        }

        public async Task<Guid> Handle(AddOrder request, CancellationToken cancellationToken)
        {
            var order = request.ToEntity();

            var customerUrl = await _serviceDiscoveryService.GetServiceUri("CustomerServices", $"/api/customers/{request.Customer.Id}");

            var httpClient = new HttpClient();
            var result = await httpClient.GetAsync(customerUrl);
            var stringRsult = await result.Content.ReadAsStringAsync();

            Console.WriteLine(stringRsult);

            await _orderRepository.AddAsync(order);

            foreach (var @event in order.Events)
            {
                var routingKey = @event.GetType().Name.ToDashCase();
                _messageBus.Publish(@event, routingKey, "order-service");
            }

            return order.Id;
        }
    }
}
