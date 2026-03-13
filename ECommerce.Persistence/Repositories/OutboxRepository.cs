using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using ECommerce.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    public class OutboxRepository : Application.Interfaces.IOutboxRepository
    {
        private readonly ECommerceDbContext _context;

        public OutboxRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OutboxMessage message)
        {
            await _context.OutboxMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }
    }
}
