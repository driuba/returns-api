using Returns.Logic.Utils;

namespace Returns.Logic.Mappings;

public sealed class ApiProfile : AutoMapper.Profile
{
    public ApiProfile()
    {
        MapDto();
        MapEntities();
    }

    private void MapDto()
    {
        CreateMap<Domain.Api.ReturnRequest, Domain.Dto.Return>();

        CreateMap<Domain.Api.ReturnLineRequest, Domain.Dto.ReturnLine>();

        CreateMap<Domain.Dto.Response, Domain.Api.Response>();

        CreateMap<Domain.Dto.ReturnValidated, Domain.Api.ReturnValidated>();

        CreateMap<Domain.Dto.ReturnLineValidated, Domain.Api.ReturnLineValidated>();

        CreateMap<Domain.Dto.ReturnEstimated, Domain.Api.ReturnEstimated>();

        CreateMap<Domain.Dto.ReturnLineEstimated, Domain.Api.ReturnLineEstimated>();

        CreateMap<Domain.Dto.ReturnFeeEstimated, Domain.Api.ReturnFeeEstimated>();
    }

    private void MapEntities()
    {
        CreateMap<Domain.Entities.FeeConfiguration, Domain.Api.FeeConfiguration>()
            .ExplicitExpansion()
            .ReverseMap();

        CreateMap<Domain.Entities.FeeConfigurationGroup, Domain.Api.FeeConfigurationGroup>().ExplicitExpansion();

        CreateMap<Domain.Entities.Return, Domain.Api.Return>()
            .ExplicitExpansion()
            .ReverseMap();

        CreateMap<Domain.Entities.ReturnAvailability, Domain.Api.ReturnAvailability>().ExplicitExpansion();

        CreateMap<Domain.Entities.ReturnFee, Domain.Api.ReturnFee>().ExplicitExpansion();

        CreateMap<Domain.Entities.ReturnLine, Domain.Api.ReturnLine>()
            .ExplicitExpansion()
            .ReverseMap();

        CreateMap<Domain.Entities.ReturnLineAttachment, Domain.Api.ReturnLineAttachment>().ExplicitExpansion();
    }
}
