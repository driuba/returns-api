using Returns.Domain.Dto;
using Returns.Domain.Entities;

namespace Returns.Domain.Services;

public interface IFeeConfigurationService
{
    Task<ValueResponse<FeeConfiguration>> Create(FeeConfiguration feeConfiguration);

    Task<ValueResponse<FeeConfiguration>> Delete(int id);

    Task<ValueResponse<FeeConfiguration>> Update(FeeConfiguration feeConfigurationCandidate);
}
