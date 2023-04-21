using Microsoft.EntityFrameworkCore;
using Returns.Domain.Dto.Invoices;
using Returns.Domain.Services;
using Returns.Logic.Mock.Repositories;

namespace Returns.Logic.Services;

public class InvoiceService : IInvoiceService
{
    private readonly MockDbContext _dbContext;
    private readonly ISessionService _sessionService;

    public InvoiceService(MockDbContext dbContext, ISessionService sessionService)
    {
        _dbContext = dbContext;
        _sessionService = sessionService;
    }

    public async Task<IEnumerable<InvoiceLine>> FilterLines(string customerId, IEnumerable<string> invoiceNumbers, IEnumerable<string> productIds)
    {
        if (string.IsNullOrEmpty(customerId))
        {
            return Enumerable.Empty<InvoiceLine>();
        }

        invoiceNumbers = invoiceNumbers.ToList();
        productIds = productIds.ToList();

        if (!(invoiceNumbers.Any() || productIds.Any()))
        {
            return Enumerable.Empty<InvoiceLine>();
        }

        var query = _dbContext
            .Set<Domain.Mock.InvoiceLine>()
            .Where(il => il.Invoice.CompanyId == _sessionService.CompanyId)
            .Where(il => il.Invoice.CustomerId == customerId);

        if (invoiceNumbers.Any())
        {
            query = query.Where(il => invoiceNumbers.Contains(il.Invoice.Number));
        }

        if (productIds.Any())
        {
            query = query.Where(il => productIds.Contains(il.ProductId));
        }

        return await query
            .Select(il => new InvoiceLine(il.Invoice.CustomerId, il.Invoice.Number, il.ProductId)
            {
                InvoiceDate = il.Invoice.Created,
                PriceUnit = il.PriceUnit,
                Quantity = il.Quantity,
                SerialNumber = il.SerialNumber
            })
            .ToListAsync();
    }
}
