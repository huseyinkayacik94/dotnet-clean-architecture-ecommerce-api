using ECommerce.Application.Common.Caching;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Queries
{
    public class GetProductByIdQuery
            : IRequest<ProductDto>, ICacheableQuery
    {
        public Guid Id { get; set; }

        public string CacheKey => $"product:{Id}";

        public int CacheTime => 5;
    }
}
