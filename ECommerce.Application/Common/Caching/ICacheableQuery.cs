using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common.Caching
{
    public interface ICacheableQuery
    {
        string CacheKey { get; }

        int CacheTime { get; }
    }
}
