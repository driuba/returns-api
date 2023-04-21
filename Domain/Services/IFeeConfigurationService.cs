using Returns.Domain.Dto;
using Returns.Domain.Entities;

namespace Returns.Domain.Services;

public interface IFeeConfigurationService
{
    Task<ValueResponse<FeeConfiguration>> Create(FeeConfiguration feeConfigurationCandidate);

    Task<ValueResponse<FeeConfiguration>> Delete(int id);

    FeeConfiguration? Resolve(IEnumerable<FeeConfiguration> configurations, string? customerId, int? countryId, IEnumerable<int> regionIds);

    Task<ValueResponse<FeeConfiguration>> Update(FeeConfiguration feeConfigurationCandidate);
}
