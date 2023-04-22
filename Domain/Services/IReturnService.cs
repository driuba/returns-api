using Returns.Domain.Dto;
using Returns.Domain.Dto.Customers;
using Returns.Domain.Dto.Invoices;
using Returns.Domain.Dto.Regions;

namespace Returns.Domain.Services;

public interface IReturnService
{
    Task<ValueResponse<Entities.Return>> CreateAsync(Return returnCandidate);

    Task<ValueResponse<Entities.Return>> DeleteAsync(int id);

    Task<ValueResponse<ReturnEstimated>> EstimateAsync(Return returnCandidate);

    Task<ValueResponse<Domain.Entities.Return>> MergeAsync(ReturnEstimated returnCandidate);

    Task<ValueResponse<Entities.Return>> UpdateAsync(Entities.Return returnCandidate);

    Task<ReturnValidated> ValidateAsync(
        Return returnCandidate,
        Country? country,
        Customer deliveryPoint,
        IEnumerable<InvoiceLine> invoiceLines,
        bool validateAttachments
    );
}
