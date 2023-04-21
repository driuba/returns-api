using Returns.Domain.Dto;

namespace Returns.Logic.Mappings;

public sealed class LogicProfile : AutoMapper.Profile
{
    public LogicProfile()
    {
        MapReturnEstimated();
        MapReturnValidated();
    }

    private void MapReturnEstimated()
    {
        CreateMap<ReturnValidated, ReturnEstimated>().ForMember(
            rv => rv.Fees,
            mce => mce.MapFrom(
                (_, _, _, ctx) => (IEnumerable<string>)ctx.Items["feesReturn"]
            )
        );

        CreateMap<ReturnLineValidated, ReturnLineEstimated>().ForMember(
            rlv => rlv.Fees,
            mce => mce.MapFrom(
                (src, _, _, ctx) =>
                {
                    var feesReturnLine = (ILookup<string, ReturnFeeEstimated>)ctx.Items["feesReturnLine"];

                    return feesReturnLine[src.Reference];
                }
            )
        );

        CreateMap<ReturnEstimated, ReturnEstimated>().ForMember(
            re => re.Fees,
            mce => mce.MapFrom(
                (_, _, _, ctx) => (IEnumerable<string>)ctx.Items["feesReturn"]
            )
        );

        CreateMap<ReturnLineEstimated, ReturnLineEstimated>().ForMember(
            rle => rle.Fees,
            mce => mce.MapFrom(
                (src, _, _, ctx) =>
                {
                    var feesReturnLine = (ILookup<string, ReturnFeeEstimated>)ctx.Items["feesReturnLine"];

                    return feesReturnLine[src.Reference];
                }
            )
        );

        CreateMap<ReturnFeeEstimated, ReturnFeeEstimated>().ForMember(
            rfe => rfe.Value,
            mce => mce.MapFrom(
                (src, _, _, ctx) =>
                {
                    if (ctx.Items["value"] is decimal value)
                    {
                        return value;
                    }

                    return src.Value;
                }
            )
        );
    }

    private void MapReturnValidated()
    {
        CreateMap<Return, ReturnValidated>().ForMember(
            r => r.Messages,
            mce => mce.MapFrom(
                (_, _, _, ctx) => (IEnumerable<string>)ctx.Items["errorsReturn"]
            )
        );

        CreateMap<ReturnLine, ReturnLineValidated>().ForMember(
            rl => rl.Messages,
            mce => mce.MapFrom(
                (src, _, _, ctx) =>
                {
                    var errorsReturnLine = (ILookup<string, string>)ctx.Items["errorsReturnLine"];

                    return errorsReturnLine[src.Reference];
                }
            )
        );
    }
}
