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

    public async Task<IEnumerable<InvoiceLine>> FilterLinesAsync(
        string customerId,
        IEnumerable<string> invoiceNumbers,
        IEnumerable<string> productIds
    )
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
            .Where(il => il.Invoice.CustomerId == customerId)
            .Where(il =>
                string.IsNullOrEmpty(_sessionService.CustomerId) ||
                il.Invoice.CustomerId == _sessionService.CustomerId
            );

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
                Id = il.Id,
                InvoiceDate = il.Invoice.Created,
                PriceUnit = il.PriceUnit,
                Quantity = il.Quantity,
                SerialNumber = il.SerialNumber
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<InvoiceLine>> FilterLinesAsync(
        string customerId,
        DateTime? from,
        string? productId,
        string? search,
        int? skip,
        DateTime? to,
        int? top
    )
    {
        var query = _dbContext
            .Set<Domain.Mock.InvoiceLine>()
            .OrderByDescending(il => il.Invoice.Created)
            .Where(il => il.Invoice.CompanyId == _sessionService.CompanyId)
            .Where(il => il.Invoice.CustomerId == customerId)
            .Where(il =>
                string.IsNullOrEmpty(_sessionService.CustomerId) ||
                il.Invoice.CustomerId == _sessionService.CustomerId
            );

        if (from.HasValue)
        {
            query = query.Where(il => il.Invoice.Created.Date >= from.Value.Date);
        }

        if (!string.IsNullOrEmpty(productId))
        {
            query = query.Where(il => il.ProductId == productId);
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(il =>
                EF.Functions.Like(il.Invoice.Number, $"%{search}%") ||
                (
                    !string.IsNullOrEmpty(il.SerialNumber) &&
                    EF.Functions.Like(il.SerialNumber, $"%{search}%")
                )
            );
        }

        if (to.HasValue)
        {
            query = query.Where(il => il.Invoice.Created.Date <= to.Value.Date);
        }

        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }

        if (top.HasValue)
        {
            query = query.Take(top.Value);
        }

        return await query
            .Select(il => new InvoiceLine(il.Invoice.CustomerId, il.Invoice.Number, il.ProductId)
            {
                Id = il.Id,
                InvoiceDate = il.Invoice.Created,
                PriceUnit = il.PriceUnit,
                Quantity = il.Quantity,
                SerialNumber = il.SerialNumber
            })
            .ToListAsync();
    }
}
