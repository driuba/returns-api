using Microsoft.EntityFrameworkCore;
using Returns.Domain.Dto.Countries;
using Returns.Domain.Enums;
using Returns.Domain.Services;

namespace Returns.Logic.Services;

public class RegionService : IRegionService
{
    private readonly Mock.Repositories.MockDbContext _dbContext;

    public RegionService(Mock.Repositories.MockDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Country?> GetCountry(int id)
    {
        return _dbContext
            .Set<Domain.Mock.Region>()
            .Where(r => r.Id == id)
            .Where(r => r.Type == RegionType.Country)
            .Select(r => new Country(r.Name)
            {
                Id = r.Id,
                Regions = r.Parents.Select(p => new Region(p.Name)
                {
                    Id = p.Id
                })
            })
            .SingleOrDefaultAsync();
    }

    public Task<Region?> GetRegion(int id)
    {
        return _dbContext
            .Set<Domain.Mock.Region>()
            .Where(r => r.Id == id)
            .Select(r => new Region(r.Name)
            {
                Id = r.Id
            })
            .SingleOrDefaultAsync();
    }
}
