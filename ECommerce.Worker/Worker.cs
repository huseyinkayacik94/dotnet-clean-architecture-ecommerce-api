using Microsoft.EntityFrameworkCore;
using ECommerce.Core.Entities;
using ECommerce.Persistence.Context;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ECommerce.Worker;

public class Worker : BackgroundService
{
    const int MAX_RETRY = 3;
    private readonly IServiceScopeFactory _scopeFactory;

    public Worker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "rabbitmq"
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.BasicQos(
            prefetchSize: 0,
            prefetchCount: 10,
            global: false);

        var args = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", "product-dlx" }
        };

        channel.ExchangeDeclare(
            exchange: "product-exchange",
            type: ExchangeType.Direct,
            durable: true);

        channel.ExchangeDeclare(
            exchange: "product-dlx",
            type: ExchangeType.Direct);

        channel.QueueDeclare(
            queue: "product-created-queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: args);

        channel.QueueDeclare(
            queue: "product-dead-letter",
            durable: true,
            exclusive: false,
            autoDelete: false);

        channel.QueueDeclare(
            queue: "product-retry-queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "product-exchange" },
                { "x-dead-letter-routing-key", "product.created" },
                { "x-message-ttl", 10000 } // 10 saniye
            });

        channel.QueueBind(
            queue: "product-created-queue",
            exchange: "product-exchange",
            routingKey: "product.created");

        channel.QueueBind(
            queue: "product-dead-letter",
            exchange: "product-dlx",
            routingKey: "product.created");

        channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            await Task.Run(async () =>
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<ECommerceDbContext>();

                try
                {
                    var messageId = ea.BasicProperties.MessageId;

                    if (string.IsNullOrEmpty(messageId))
                    {
                        messageId = Guid.NewGuid().ToString();
                    }

                    var exists = await dbContext.InboxMessages
                                 .AnyAsync(x => x.MessageId == messageId);

                    if (exists)
                    {
                        channel.BasicAck(ea.DeliveryTag, false);
                        return;
                    }

                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine("Event received:");
                    Console.WriteLine(message);

                    dbContext.InboxMessages.Add(new InboxMessage
                    {
                        Id = Guid.NewGuid(),
                        MessageId = messageId,
                        ProcessedAt = DateTime.UtcNow
                    });

                    await dbContext.SaveChangesAsync();

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception)
                {
                    int retryCount = 0;

                    if (ea.BasicProperties.Headers != null &&
                        ea.BasicProperties.Headers.TryGetValue("x-retry-count", out var value))
                    {
                        retryCount = int.Parse(Encoding.UTF8.GetString((byte[])value));
                    }

                    retryCount++;

                    if (retryCount >= 3)
                    {
                        Console.WriteLine("Message moved to DLQ");

                        channel.BasicPublish(
                            exchange: "product-dlx",
                            routingKey: "product.created",
                            body: ea.Body);

                        channel.BasicAck(ea.DeliveryTag, false);
                        return;
                    }

                    var props = channel.CreateBasicProperties();

                    props.Headers = new Dictionary<string, object>
                {
                    { "x-retry-count", Encoding.UTF8.GetBytes((retryCount + 1).ToString()) }
                };

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "product-retry-queue",
                        basicProperties: props,
                        body: ea.Body);

                    channel.BasicAck(ea.DeliveryTag, false);
                }
            });
        };

        channel.BasicConsume(
            queue: "product-created-queue",
            autoAck: false,
            consumer: consumer);

        return Task.CompletedTask;
    }
}

