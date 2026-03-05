using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Guid> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product(dto.Name, dto.Price, dto.Stock);

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return product.Id;
        }

        public async Task<Product?> GetProductAsync(Guid id)
        {
            return await _productRepository.GetByIdAsync(id);
        }
    }
}
