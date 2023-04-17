using Returns.Domain.Dto;

namespace Returns.Domain.Services;

public interface IStorageService
{
    Task<Response> Delete(params Guid[] ids);

    Task<Response> Delete(IEnumerable<Guid> ids);
}
