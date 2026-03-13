using Microsoft.AspNetCore.Hosting;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class RabbitMQPublisher
    {
        public void Publish<T>(T message, string routingKey)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: "product-exchange",
                type: ExchangeType.Direct,
                durable:true);

            var json = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(json);

            var props = channel.CreateBasicProperties();

            props.MessageId = Guid.NewGuid().ToString();
            props.Persistent = true;

            props.Headers = new Dictionary<string, object>
            {
                { "retry-count", 0 }
            };

            channel.BasicPublish(
                exchange: "product-exchange",
                routingKey: routingKey,
                basicProperties: props,
                body: body);
        }
    }
}
