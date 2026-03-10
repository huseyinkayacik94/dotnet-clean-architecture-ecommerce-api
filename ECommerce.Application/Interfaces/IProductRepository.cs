using ECommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<Product?> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
        Task<List<Product>> GetAllAsync();

        Task<List<Product>> GetPagedAsync(int page, int pageSize);
    }
}
