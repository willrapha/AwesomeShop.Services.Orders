using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeShop.Services.Orders.Infrastructure.ServiceDiscovery
{
    public class ConsulService : IServiceDiscoveryService
    {
        private readonly IConsulClient _consulClient;

        public ConsulService(IConsulClient consulClient)
        {
            _consulClient = consulClient;
        }

        public async Task<Uri> GetServiceUri(string serviceName, string requestUrl)
        {
            var allRegisteredService = await _consulClient.Agent.Services(); // Pegar todos os servicos registrados

            var registeredService = allRegisteredService.Response?
                .Where(s => s.Value.Service.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase))
                .Select(s => s.Value)
                .ToList();

            var service = registeredService?.FirstOrDefault();

            Console.WriteLine($"Service: {service.Service}");

            var uri = new Uri($"http://{service.Address}:{service.Port}{requestUrl}");

            return uri;
        }
    }
}
