using ECommerce.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Queries
{
    public class GetProductsQuery : IRequest<List<ProductDto>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
