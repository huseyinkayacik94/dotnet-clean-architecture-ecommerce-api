using ECommerce.Application.Events;
using ECommerce.Application.Features.Products.Commands;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Handlers
{
    public class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly IOutboxRepository _outboxRepository;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IOutboxRepository outboxRepository)
        {
            _productRepository = productRepository;
            _outboxRepository = outboxRepository;
        }

        public async Task<Guid> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            Console.WriteLine("Handler çalıştı");

            var product = new Product(
                request.Name,
                request.Price,
                0);

            await _productRepository.AddAsync(product);

            var productEvent = new ProductCreatedEvent
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = "ProductCreated",
                Payload = JsonSerializer.Serialize(productEvent),
                CreatedAt = DateTime.UtcNow,
                Processed = false
            };

            await _outboxRepository.AddAsync(outboxMessage);

            return product.Id;
        }
    }
}
