using ECommerce.Application.Services;
using ECommerce.OutboxWorker;
using ECommerce.Persistence.Context;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure();
        }));

builder.Services.AddSingleton<RabbitMQPublisher>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
