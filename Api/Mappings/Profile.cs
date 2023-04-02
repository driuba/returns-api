using Returns.Api.Utils;

namespace Returns.Api.Mappings;

public class Profile : AutoMapper.Profile
{
    public Profile()
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

        CreateMap<Domain.Entities.ReturnLineDevice, Domain.Api.ReturnLineDevice>().ExplicitExpansion();

        CreateMap<Domain.Logic.Response, Domain.Api.Response>();
    }
}
