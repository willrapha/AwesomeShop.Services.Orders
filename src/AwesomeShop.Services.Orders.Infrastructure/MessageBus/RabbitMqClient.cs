using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using System.Text;

namespace AwesomeShop.Services.Orders.Infrastructure.MessageBus
{
    public class RabbitMqClient : IMessageBusClient
    {
        private readonly IConnection _connection;
        
        public RabbitMqClient(ProducerConnection producerConnection)
        {
            _connection = producerConnection.Connection;
        }
        
        public void Publish(object message, string routingKey, string exchange)
        {
            var channel = _connection.CreateModel(); // criando canal de comunicação

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver() // nome das propriedades em camelCase
            };                

            var payload = JsonConvert.SerializeObject(message, settings); // serializando o objeto
            var body = Encoding.UTF8.GetBytes(payload); // transformando o objeto em bytes

            channel.ExchangeDeclare(exchange, "topic", true); 

            channel.BasicPublish(exchange, routingKey, null, body); // publicando o objeto
        }
    }
}
