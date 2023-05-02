using Returns.Domain.Dto;

namespace Returns.Domain.Services;

public interface IReturnLineService
{
    Task<ValueResponse<IEnumerable<Entities.ReturnLine>>> CreateAsync(int returnId, IEnumerable<ReturnLine> returnLinesCandidate);

    Task<Response> DeclineAsync(int returnId, IEnumerable<int> returnLineIds, string note);

    Task<ValueResponse<Entities.ReturnLine>> DeleteAsync(int returnId, int returnLineId);

    Task<Response> InvoiceAsync(int returnId, IEnumerable<int> returnLineIds);

    Task<ValueResponse<Entities.ReturnLine>> UpdateAsync(int returnId, ReturnLine returnLineCandidate);
}
