using Returns.Domain.Dto;
using Returns.Domain.Entities;

namespace Returns.Domain.Services;

public interface IReturnAvailabilityService
{
    Task<ValueResponse<ReturnAvailability>> GetAsync(string deliveryPointId);
}
