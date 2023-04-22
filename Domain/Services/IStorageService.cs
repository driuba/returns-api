using Returns.Domain.Dto;

namespace Returns.Domain.Services;

public interface IStorageService
{
    Task<ValueResponse<Guid?>> CreateAsync(Stream stream);

    Response Delete(params Guid[] ids);

    Response Delete(IEnumerable<Guid> ids);

    ValueResponse<Stream> Get(Guid id);
}
