using Returns.Domain.Dto.Invoices;

namespace Returns.Domain.Services;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceLine>> FilterLinesAsync(string customerId, IEnumerable<string> invoiceNumbers, IEnumerable<string> productIds);

    Task<IEnumerable<InvoiceLine>> FilterLinesAsync(
        string customerId,
        DateTime? from,
        string? productId,
        string? search,
        int? skip,
        DateTime? to,
        int? top
    );
}
