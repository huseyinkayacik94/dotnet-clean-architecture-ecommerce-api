using ECommerce.Application.Common.Caching;
using ECommerce.Application.Features.Products.Queries;
using ECommerce.Application.Interfaces;
using MediatR;

public class GetProductsQueryHandler
    : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _repository;
    private readonly RedisCacheService _cache;

    public GetProductsQueryHandler(
        IProductRepository repository,
        RedisCacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<List<ProductDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"products_page_{request.Page}_{request.PageSize}";

        var cached = await _cache.GetAsync<List<ProductDto>>(cacheKey);

        if (cached != null)
        {
            Console.WriteLine("CACHE HIT - Redis");
            return cached;
        }

        Console.WriteLine("CACHE MISS - DB");

        var products = await _repository.GetPagedAsync(
            request.Page,
            request.PageSize);

        var result = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price
        }).ToList();

        await _cache.SetAsync(cacheKey, result);

        return result;
    }
}