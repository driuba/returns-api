using Microsoft.EntityFrameworkCore;
using Returns.Domain.Dto.Products;
using Returns.Domain.Services;
using Returns.Logic.Mock.Repositories;

namespace Returns.Logic.Services;

public class ProductService : IProductService
{
    private readonly MockDbContext _dbContext;

    public ProductService(MockDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> FilterAsync(IEnumerable<string> ids)
    {
        ids = ids.ToList();

        if (!ids.Any())
        {
            return Enumerable.Empty<Product>();
        }

        return await _dbContext
            .Set<Domain.Mock.Product>()
            .Where(p => ids.Contains(p.Id))
            .Select(p => new Product(p.Id)
            {
                ByOrderOnly = p.ByOrderOnly,
                Serviceable = p.Serviceable
            })
            .ToListAsync();
    }
}
