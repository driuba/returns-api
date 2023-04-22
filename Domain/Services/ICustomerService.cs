using Returns.Domain.Dto.Customers;

namespace Returns.Domain.Services;

public interface ICustomerService
{
    Task<Customer?> GetDeliveryPointAsync(string deliveryPointId);
}
