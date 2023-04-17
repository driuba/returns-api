using Returns.Domain.Dto;

namespace Returns.Domain.Services;

public interface IReturnService
{
    Task<ValueResponse<Entities.Return>> Create(Return returnCandidate);

    Task<ValueResponse<Entities.Return>> Delete(int id);

    Task<ValueResponse<Entities.Return>> Update(Entities.Return returnCandidate);

    Task<ReturnValidated> Validate(Return returnCandidate);
}
