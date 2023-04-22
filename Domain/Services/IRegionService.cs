using Returns.Domain.Dto.Regions;

namespace Returns.Domain.Services;

public interface IRegionService
{
    Task<Country?> GetCountryAsync(int id);

    Task<Region?> GetRegionAsync(int id);
}
