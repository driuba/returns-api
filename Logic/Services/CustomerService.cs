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

    public Task<Customer?> GetDeliveryPoint(string deliveryPointId)
    {
        return _dbContext
            .Set<Domain.Mock.Customer>()
            .Where(c => c.CompanyId == _sessionService.CompanyId)
            .Where(c => c.Id == deliveryPointId)
            .Select(c => new Customer(c.ParentId ?? c.Id, c.Id, c.Name)
            {
                CountryId = c.CountryId
            })
            .SingleOrDefaultAsync();
    }
}
