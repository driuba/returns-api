using Returns.Domain.Dto.Invoices;

namespace Returns.Domain.Services;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceLine>> FilterLines(IEnumerable<string> invoiceNumbers, IEnumerable<string> productIds);
}
