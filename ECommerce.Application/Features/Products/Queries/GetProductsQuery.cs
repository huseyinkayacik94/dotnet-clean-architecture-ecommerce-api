using ECommerce.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Queries
{
    public class GetProductsQuery : IRequest<PagedResult<ProductDto>>
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
