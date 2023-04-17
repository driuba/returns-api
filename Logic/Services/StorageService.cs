using Returns.Domain.Dto;
using Returns.Domain.Services;

namespace Returns.Logic.Services;

public class StorageService : IStorageService
{
    public Task<Response> Delete(params Guid[] ids)
    {
        return Delete(ids.AsEnumerable());
    }

    public Task<Response> Delete(IEnumerable<Guid> ids)
    {
        throw new NotImplementedException();
    }
}
