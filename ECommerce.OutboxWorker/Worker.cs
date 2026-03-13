using ECommerce.Application.Services;
using ECommerce.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.OutboxWorker
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMQPublisher _publisher;

        public Worker(IServiceProvider serviceProvider, RabbitMQPublisher publisher)
        {
            _serviceProvider = serviceProvider;
            _publisher = publisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Outbox Worker running...");

                using var scope = _serviceProvider.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();

                var messages = await db.OutboxMessages
                    .Where(x => !x.Processed)
                    .OrderBy(x => x.CreatedAt)
                    .Take(20)
                    .ToListAsync();

                Console.WriteLine($"Found {messages.Count} outbox messages");

                foreach (var message in messages)
                {
                    try
                    {
                        Console.WriteLine("Publishing to RabbitMQ...");

                        _publisher.Publish(message.Payload, "product.created");

                        message.Processed = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Outbox error: " + ex.Message);
                    }
                }

                await db.SaveChangesAsync();

                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
