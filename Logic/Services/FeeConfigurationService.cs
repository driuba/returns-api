using Microsoft.EntityFrameworkCore;
using Returns.Domain.Entities;
using Returns.Domain.Enums;
using Returns.Domain.Logic;
using Returns.Domain.Services;
using Returns.Logic.Repositories;

namespace Returns.Logic.Services;

public class FeeConfigurationService : IFeeConfigurationService
{
    private readonly ReturnDbContext _dbContext;

    public FeeConfigurationService(ReturnDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ValueResponse<FeeConfiguration>> Create(FeeConfiguration feeConfiguration)
    {
        if (string.IsNullOrEmpty(feeConfiguration.CountryId) && string.IsNullOrEmpty(feeConfiguration.CustomerId))
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = "Either customer or country / region must be provided."
            };
        }

        if (!(string.IsNullOrEmpty(feeConfiguration.CountryId) || string.IsNullOrEmpty(feeConfiguration.CustomerId)))
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = "Only one of customer or country / region must be provided."
            };
        }

        if (feeConfiguration.Value < 0)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = "Value must be a non-negative number."
            };
        }

        if (feeConfiguration is { ValueType: FeeValueType.Fixed, ValueMinimum: { } })
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = "Minimum value cannot be defined for fixed fees."
            };
        }

        if (feeConfiguration.ValueMinimum <= 0)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = "Minimum value must either be empty or a positive number."
            };
        }

        // TODO: add mock country existence validation

        // TODO: add mock customer existence validation

        var feeConfigurationGroup = await _dbContext
            .Set<FeeConfigurationGroup>()
            .SingleOrDefaultAsync(fcg => fcg.Id == feeConfiguration.FeeConfigurationGroupId);

        if (feeConfigurationGroup is null)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = $"Fee configuration group {feeConfiguration.FeeConfigurationGroupId} was not found."
            };
        }

        var feeConfigurationExists = await _dbContext
            .Set<FeeConfiguration>()
            .Where(fc => !fc.Deleted)
            .Where(fc => string.IsNullOrEmpty(feeConfiguration.CountryId) || fc.CountryId == feeConfiguration.CountryId)
            .Where(fc => string.IsNullOrEmpty(feeConfiguration.CustomerId) || fc.CustomerId == feeConfiguration.CustomerId)
            .AnyAsync(fc => fc.FeeConfigurationGroupId == feeConfiguration.FeeConfigurationGroupId);

        if (feeConfigurationExists)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message =
                    $"Fee configuration group {feeConfigurationGroup.Name} already has fee configuration for " +
                    $"{(string.IsNullOrEmpty(feeConfiguration.CustomerId) ? feeConfiguration.CountryId : feeConfiguration.CustomerId)}."
            };
        }

        _dbContext
            .Set<FeeConfiguration>()
            .Add(feeConfiguration);

        await _dbContext.SaveChangesAsync();

        return new ValueResponse<FeeConfiguration>
        {
            Success = true,
            Value = feeConfiguration
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

        if (string.IsNullOrEmpty(feeConfiguration.CountryId) && string.IsNullOrEmpty(feeConfiguration.CustomerId))
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

    public async Task<ValueResponse<FeeConfiguration>> Update(FeeConfiguration feeConfigurationCandidate)
    {
        if (feeConfigurationCandidate.Value < 0)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = "Value must be a non-negative number."
            };
        }

        if (feeConfigurationCandidate is { ValueType: FeeValueType.Fixed, ValueMinimum: { } })
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

        var feeConfiguration = await _dbContext
            .Set<FeeConfiguration>()
            .AsTracking()
            .Where(fc => !fc.Deleted)
            .SingleOrDefaultAsync(fc => fc.Id == feeConfigurationCandidate.Id);

        if (feeConfiguration is null)
        {
            return new ValueResponse<FeeConfiguration>
            {
                Message = $"Fee configuration {feeConfigurationCandidate.Id} was not found."
            };
        }

        feeConfiguration.Value = feeConfigurationCandidate.Value;
        feeConfiguration.ValueMinimum = feeConfigurationCandidate.ValueMinimum;
        feeConfiguration.ValueType = feeConfigurationCandidate.ValueType;

        await _dbContext.SaveChangesAsync();

        return new ValueResponse<FeeConfiguration>
        {
            Success = true,
            Value = feeConfiguration
        };
    }
}
