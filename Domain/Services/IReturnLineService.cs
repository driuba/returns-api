using Returns.Domain.Dto;

namespace Returns.Domain.Services;

public interface IReturnLineService
{
    Task<ValueResponse<IEnumerable<Entities.ReturnLine>>> CreateAsync(int returnId, IEnumerable<ReturnLine> returnLinesCandidate);

    Task<ValueResponse<Entities.ReturnLine>> DeleteAsync(int returnId, int returnLineId);

    Task<ValueResponse<Entities.ReturnLine>> UpdateAsync(int returnId, ReturnLine returnLineCandidate);
}
