using Microsoft.EntityFrameworkCore;
using Returns.Domain.Dto.Customers;
using Returns.Domain.Services;
using Returns.Logic.Mock.Repositories;

namespace Returns.Logic.Services;

public class CustomerService : ICustomerService
{
    private readonly MockDbContext _dbContext;
    private readonly ISessionService _sessionService;

    public CustomerService(MockDbContext dbContext, ISessionService sessionService)
    {
        _dbContext = dbContext;
        _sessionService = sessionService;
    }

    public async Task<Customer?> GetDeliveryPointAsync(string deliveryPointId)
    {
        if (string.IsNullOrEmpty(deliveryPointId))
        {
            return default(Customer?);
        }

        return await _dbContext
            .Set<Domain.Mock.Customer>()
            .Where(c =>
                string.IsNullOrEmpty(c.ParentId)
                    ? c.Companies.Any(cc => cc.CompanyId == _sessionService.CompanyId)
                    : c.Parent!.Companies.Any(cc => cc.CompanyId == _sessionService.CompanyId)
            )
            .Where(c =>
                string.IsNullOrEmpty(_sessionService.CustomerId) ||
                c.Id == _sessionService.CustomerId ||
                c.ParentId == _sessionService.CustomerId
            )
            .Where(c => c.Id == deliveryPointId)
            .Select(c => new Customer(c.ParentId ?? c.Id, c.Id, c.Name)
            {
                CountryId = string.IsNullOrEmpty(c.ParentId) ? c.CountryId : c.Parent!.CountryId
            })
            .SingleOrDefaultAsync();
    }
}
