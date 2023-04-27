using Microsoft.EntityFrameworkCore;
using Returns.Domain.Dto;
using Returns.Domain.Entities;
using Returns.Domain.Services;
using Returns.Logic.Repositories;

namespace Returns.Logic.Services;

public class ReturnAvailabilityService : IReturnAvailabilityService
{
    private readonly ICustomerService _customerService;
    private readonly ReturnDbContext _dbContext;
    private readonly IRegionService _regionService;

    public ReturnAvailabilityService(ICustomerService customerService, ReturnDbContext dbContext, IRegionService regionService)
    {
        _customerService = customerService;
        _dbContext = dbContext;
        _regionService = regionService;
    }

    public async Task<ValueResponse<ReturnAvailability>> GetAsync(string deliveryPointId)
    {
        var deliveryPoint = await _customerService.GetDeliveryPointAsync(deliveryPointId);

        if (deliveryPoint is null)
        {
            return new ValueResponse<ReturnAvailability>
            {
                Message = $"Delivery point {deliveryPointId} was not found."
            };
        }

        var country = await _regionService.GetCountryAsync(deliveryPoint.CountryId);

        var countryId = country?.Id;
        var regionIds = country?.Regions.Select(r => r.Id) ?? Enumerable.Empty<int>();

        if (countryId.HasValue)
        {
            regionIds = regionIds.Append(countryId.Value);
        }

        regionIds = regionIds.ToList();

        var returnAvailabilities = await _dbContext
            .Set<ReturnAvailability>()
            .Where(ra =>
                !ra.RegionId.HasValue ||
                // ReSharper disable once AccessToModifiedClosure
                regionIds.Contains(ra.RegionId.Value)
            )
            .ToDictionaryAsync(ra => ra.RegionId ?? default(int));

        var response = new ValueResponse<ReturnAvailability>
        {
            Success = true
        };

        if (countryId.HasValue && returnAvailabilities.TryGetValue(countryId.Value, out var returnAvailability))
        {
            response.Value = returnAvailability;
        }
        else
        {
            regionIds = regionIds
                .Intersect(returnAvailabilities.Keys)
                .ToList();

            response.Value = returnAvailabilities[regionIds.Any() ? regionIds.First() : default(int)];
        }

        return response;
    }
}
