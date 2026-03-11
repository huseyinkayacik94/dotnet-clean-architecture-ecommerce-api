using ECommerce.Application.Common.Caching;
using ECommerce.Application.Features.Products.Queries;
using ECommerce.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Handlers
{
    public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductRepository _repository;
        private readonly RedisCacheService _cache;

        public GetProductByIdQueryHandler(
            IProductRepository repository,
            RedisCacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"product:{request.Id}";

            var cachedProduct = await _cache.GetAsync<ProductDto>(cacheKey);

            if (cachedProduct != null)
            {
                Console.WriteLine("CACHE HIT → Redis");
                return cachedProduct;
            }

            Console.WriteLine("CACHE MISS → DB");

            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null)
                return null;

            ProductDto productDto = new ProductDto();
            productDto.Id = product.Id;
            productDto.Name = product.Name;
            productDto.Price = product.Price;

            await _cache.SetAsync(cacheKey, product, 5);

            return productDto;
        }
    }
}
