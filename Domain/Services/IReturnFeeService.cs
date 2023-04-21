using Returns.Domain.Dto;
using Returns.Domain.Dto.Customers;
using Returns.Domain.Dto.Invoices;
using Returns.Domain.Dto.Regions;

namespace Returns.Domain.Services;

public interface IReturnFeeService
{
    ReturnEstimated Calculate(ReturnEstimated returnEstimated, IEnumerable<InvoiceLine> invoiceLines);

    Task<ReturnEstimated> Resolve(
        ReturnValidated returnValidated,
        Country? country,
        Customer deliveryPoint,
        IEnumerable<InvoiceLine> invoiceLines
    );
}
