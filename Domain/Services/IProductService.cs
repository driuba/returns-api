using Returns.Domain.Dto.Products;

namespace Returns.Domain.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> Filter(IEnumerable<string> ids);
}
