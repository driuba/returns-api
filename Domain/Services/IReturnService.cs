using Returns.Domain.Dto;
using Returns.Domain.Dto.Customers;
using Returns.Domain.Dto.Invoices;
using Returns.Domain.Dto.Regions;

namespace Returns.Domain.Services;

public interface IReturnService
{
    Task<ValueResponse<Entities.Return>> Create(Return returnCandidate);

    Task<ValueResponse<Entities.Return>> Delete(int id);

    Task<ReturnEstimated> Estimate(Return returnCandidate);

    Task<ValueResponse<Entities.Return>> Update(Entities.Return returnCandidate);

    Task<ReturnValidated> Validate(
        Return returnCandidate,
        Country? country,
        Customer deliveryPoint,
        IEnumerable<InvoiceLine> invoiceLines,
        bool validateAttachments
    );
}
