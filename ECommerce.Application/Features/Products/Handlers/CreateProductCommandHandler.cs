using ECommerce.Application.Features.Products.Commands;
using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Handlers
{
    public class CreateProductCommandHandler
        : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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

            return product.Id;
        }
    }
}
