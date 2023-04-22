using Returns.Domain.Dto;

namespace Returns.Domain.Services;

public interface IReturnLineAttachmentService
{
    Task<ValueResponse<Entities.ReturnLineAttachment>> CreateAsync(int returnId, int returnLineId, Stream file, string name);

    Task<ValueResponse<Attachment>> GetAsync(int returnId, int returnLineId, Guid storageId);

    Task<ValueResponse<Entities.ReturnLineAttachment>> DeleteAsync(int returnId, int returnLineId, Guid storageId);
}
