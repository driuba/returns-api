using Returns.Domain.Dto.Countries;

namespace Returns.Domain.Services;

public interface IRegionService
{
    Task<Country?> GetCountry(int id);

    Task<Region?> GetRegion(int id);
}
