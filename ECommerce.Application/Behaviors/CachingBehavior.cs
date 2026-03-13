using ECommerce.Application.Common.Caching;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Behaviors
{
    public class CachingBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly RedisCacheService _cache;

        public CachingBehavior(RedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (request is not ICacheableQuery cacheableQuery)
                return await next();

            var cachedResponse = await _cache.GetAsync<TResponse>(cacheableQuery.CacheKey);

            if (cachedResponse != null)
                return cachedResponse;

            var response = await next();

            await _cache.SetAsync(
                cacheableQuery.CacheKey,
                response,
                cacheableQuery.CacheTime);

            return response;
        }
    }
}
