using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Returns.Domain.Dto;
using Returns.Domain.Entities;
using Returns.Domain.Enums;
using Returns.Domain.Services;
using Returns.Logic.Repositories;

namespace Returns.Logic.Services;

public class FeeConfigurationService : IFeeConfigurationService
{
    private readonly ICustomerService _customerService;
    private readonly ReturnDbContext _dbContext;
    private readonly IRegionService _regionService;

    public FeeConfigurationService(ICustomerService customerService, ReturnDbContext dbContext, IRegionService regionService)
    {
        _customerService = customerService;
        _dbContext = dbContext;
        _regionService = regionService;
    }

    public async Task<ValueResponse<FeeConfiguration>> Create(FeeConfiguration feeConfigurationCandidate)
    {
        if (string.IsNullOrEmpty(feeConfigurationCandidate.CustomerId) && !feeConfigurationCandidate.RegionId.HasValue)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = "Either customer or country / region must be provided."
            };
        }

        if (!string.IsNullOrEmpty(feeConfigurationCandidate.CustomerId) && feeConfigurationCandidate.RegionId.HasValue)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = "Only one of customer or country / region must be provided."
            };
        }

        if (feeConfigurationCandidate.Value < 0)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = "Value must be a non-negative number."
            };
        }

        if (feeConfigurationCandidate is { ValueType: FeeValueType.Fixed, ValueMinimum: not null })
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = "Minimum value cannot be defined for fixed fees."
            };
        }

        if (feeConfigurationCandidate.ValueMinimum <= 0)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = "Minimum value must either be empty or a positive number."
            };
        }

        if (feeConfigurationCandidate.RegionId.HasValue)
        {
            var region = await _regionService.GetRegion(feeConfigurationCandidate.RegionId.Value);

            if (region is null)
            {
                return new ValueResponse<FeeConfiguration>
                {
                    Message = $"Region {feeConfigurationCandidate.RegionId:D3} was not found."
                };
            }
        }

        if (!string.IsNullOrEmpty(feeConfigurationCandidate.CustomerId))
        {
            var deliveryPoint = await _customerService.GetDeliveryPoint(feeConfigurationCandidate.CustomerId);

            if (deliveryPoint is null)
            {
                return new ValueResponse<FeeConfiguration>
                {
                    Message = $"Customer {feeConfigurationCandidate.CustomerId} was not found."
                };
            }

            if (!string.Equals(deliveryPoint.CustomerId, deliveryPoint.Id, StringComparison.OrdinalIgnoreCase))
            {
                return new ValueResponse<FeeConfiguration>
                {
                    Message =
                        $"Customer {feeConfigurationCandidate.CustomerId} ({deliveryPoint.Name}) is not a parent customer, " +
                        $"only parent customers may have fees configured."
                };
            }
        }

        var feeConfigurationGroup = await _dbContext
            .Set<FeeConfigurationGroup>()
            .SingleOrDefaultAsync(fcg => fcg.Id == feeConfigurationCandidate.FeeConfigurationGroupId);

        if (feeConfigurationGroup is null)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = $"Fee configuration group {feeConfigurationCandidate.FeeConfigurationGroupId} was not found."
            };
        }

        var feeConfigurationExists = await _dbContext
            .Set<FeeConfiguration>()
            .Where(fc => !fc.Deleted)
            .Where(fc => !feeConfigurationCandidate.RegionId.HasValue || fc.RegionId == feeConfigurationCandidate.RegionId)
            .Where(fc => string.IsNullOrEmpty(feeConfigurationCandidate.CustomerId) || fc.CustomerId == feeConfigurationCandidate.CustomerId)
            .AnyAsync(fc => fc.FeeConfigurationGroupId == feeConfigurationCandidate.FeeConfigurationGroupId);

        if (feeConfigurationExists)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message =
                    $"Fee configuration group {feeConfigurationGroup.Name} already has fee configuration for " +
                    $"{(string.IsNullOrEmpty(feeConfigurationCandidate.CustomerId) ? $"{feeConfigurationCandidate.RegionId:D3}" : feeConfigurationCandidate.CustomerId)}."
            };
        }

        _dbContext
            .Set<FeeConfiguration>()
            .Add(feeConfigurationCandidate);

        await _dbContext.SaveChangesAsync();

        return new ValueResponse<FeeConfiguration>
        {
            Success = true,
            Value = feeConfigurationCandidate
        };
    }

    public async Task<ValueResponse<FeeConfiguration>> Delete(int id)
    {
        var feeConfiguration = await _dbContext
            .Set<FeeConfiguration>()
            .AsTracking()
            .SingleOrDefaultAsync(fc => fc.Id == id);

        if (feeConfiguration is null)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = $"Fee configuration {id} was not found."
            };
        }

        if (feeConfiguration.Deleted)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = $"Fee configuration {feeConfiguration.Id} is already deleted."
            };
        }

        if (string.IsNullOrEmpty(feeConfiguration.CustomerId) && !feeConfiguration.RegionId.HasValue)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = "Default fee configuration cannot be deleted."
            };
        }

        feeConfiguration.Deleted = true;

        await _dbContext.SaveChangesAsync();

        return new ValueResponse<FeeConfiguration>
        {
            Success = true,
            Value = feeConfiguration
        };
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public FeeConfiguration? Resolve(IEnumerable<FeeConfiguration> configurations, string? customerId, int? countryId, IEnumerable<int> regionIds)
    {
        configurations = configurations.ToList();

        if (!string.IsNullOrEmpty(customerId))
        {
            var configuration = configurations.SingleOrDefault(c => string.Equals(c.CustomerId, customerId, StringComparison.OrdinalIgnoreCase));

            if (configuration is not null)
            {
                return configuration;
            }
        }

        if (countryId.HasValue)
        {
            var configuration = configurations.SingleOrDefault(c => c.RegionId == countryId);

            if (configuration is not null)
            {
                return configuration;
            }
        }

        foreach (var regionId in regionIds)
        {
            var configuration = configurations.SingleOrDefault(c => c.RegionId == regionId);

            if (configuration is not null)
            {
                return configuration;
            }
        }

        return configurations.SingleOrDefault(c =>
            !c.RegionId.HasValue &&
            string.IsNullOrEmpty(c.CustomerId)
        );
    }

    public async Task<ValueResponse<FeeConfiguration>> Update(FeeConfiguration feeConfigurationCandidate)
    {
        if (feeConfigurationCandidate.Value < 0)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = "Value must be a non-negative number."
            };
        }

        if (feeConfigurationCandidate is { ValueType: FeeValueType.Fixed, ValueMinimum: not null })
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = "Minimum value cannot be defined for fixed fees."
            };
        }

        if (feeConfigurationCandidate.ValueMinimum <= 0)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = "Minimum value must either be empty or a positive number."
            };
        }

        var feeConfigurationExisting = await _dbContext
            .Set<FeeConfiguration>()
            .AsTracking()
            .Where(fc => !fc.Deleted)
            .SingleOrDefaultAsync(fc => fc.Id == feeConfigurationCandidate.Id);

        if (feeConfigurationExisting is null)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = $"Fee configuration {feeConfigurationCandidate.Id} was not found."
            };
        }

        feeConfigurationExisting.Value = feeConfigurationCandidate.Value;
        feeConfigurationExisting.ValueMinimum = feeConfigurationCandidate.ValueMinimum;
        feeConfigurationExisting.ValueType = feeConfigurationCandidate.ValueType;

        await _dbContext.SaveChangesAsync();

        return new ValueResponse<FeeConfiguration>
        {
            Success = true,
            Value = feeConfigurationExisting
        };
    }
}
