using Returns.Domain.Dto;

namespace Returns.Domain.Services;

public interface IStorageService
{
    Task<ValueResponse<Guid>> Create(Stream stream);

    Response Delete(params Guid[] ids);

    Response Delete(IEnumerable<Guid> ids);

    ValueResponse<Stream> Get(Guid id);
}
