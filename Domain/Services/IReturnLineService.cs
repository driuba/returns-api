using Returns.Domain.Dto;

namespace Returns.Domain.Services;

public interface IReturnLineService
{
    Task<ValueResponse<Entities.ReturnLine>> CreateAsync(int returnId, ReturnLine returnLineCandidate);

    Task<ValueResponse<Entities.ReturnLine>> DeleteAsync(int returnId, int returnLineId);

    Task<ValueResponse<Entities.ReturnLine>> UpdateAsync(int returnId, ReturnLine returnLineCandidate);
}
