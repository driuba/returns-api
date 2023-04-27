using Returns.Domain.Dto;

namespace Returns.Logic.Mappings;

public sealed class LogicProfile : AutoMapper.Profile
{
    public LogicProfile()
    {
        MapReturnEntity();
        MapReturnEstimated();
        MapReturnValidated();
    }

    private void MapReturnEntity()
    {
        var returnMap = CreateMap<ReturnEstimated, Domain.Entities.Return>();

        returnMap.ConstructUsing((src, ctx) =>
        {
            if (!(ctx.Items.TryGetValue("companyId", out var item) && item is string companyId))
            {
                throw new InvalidOperationException("Company identifier is required.");
            }

            if (!(ctx.Items.TryGetValue("customerId", out item) && item is string customerId))
            {
                throw new InvalidOperationException("Customer identifier is required.");
            }

            return new Domain.Entities.Return(companyId, customerId, src.DeliveryPointId, string.Empty);
        });

        var returnFeeMap = CreateMap<ReturnFeeEstimated, Domain.Entities.ReturnFee>();

        returnFeeMap.ForMember(
            rf => rf.ReturnId,
            mce => mce.MapFrom(
                (_, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("returnId", out var item) && item is int returnId)
                    {
                        return returnId;
                    }

                    return default(int);
                }
            )
        );

        returnFeeMap.ForMember(
            rf => rf.ReturnLineId,
            mce => mce.MapFrom(
                (_, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("returnLineId", out var item) && item is int returnLineId)
                    {
                        return returnLineId;
                    }

                    return default(int?);
                }
            )
        );

        var returnLineMap = CreateMap<ReturnLineEstimated, Domain.Entities.ReturnLine>();

        returnLineMap.ForCtorParam(
            "invoiceNumberPurchase",
            cpce => cpce.MapFrom(src => src.InvoiceNumber)
        );

        returnLineMap.ForMember(
            rl => rl.NoteReturn,
            mce => mce.MapFrom(
                (_, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("noteReturn", out var item) && item is string noteReturn)
                    {
                        return noteReturn;
                    }

                    return default(string?);
                }
            )
        );

        returnLineMap.ForMember(
            rl => rl.NoteResponse,
            mce => mce.MapFrom(
                (_, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("noteResponse", out var item) && item is string noteResponse)
                    {
                        return noteResponse;
                    }

                    return default(string?);
                }
            )
        );

        returnLineMap.ForMember(
            rl => rl.ReturnId,
            mce => mce.MapFrom(
                (_, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("returnId", out var item) && item is int returnId)
                    {
                        return returnId;
                    }

                    return default(int);
                }
            )
        );
    }

    private void MapReturnEstimated()
    {
        CreateMap<ReturnValidated, ReturnEstimated>().ForMember(
            rv => rv.Fees,
            mce => mce.MapFrom(
                (_, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("feesReturn", out var item) && item is IEnumerable<string> feesReturn)
                    {
                        return feesReturn;
                    }

                    throw new InvalidOperationException("Return fees are required.");
                }
            )
        );

        CreateMap<ReturnLineValidated, ReturnLineEstimated>().ForMember(
            rlv => rlv.Fees,
            mce => mce.MapFrom(
                (src, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("feesReturnLine", out var item) && item is ILookup<string, ReturnFeeEstimated> feesReturnLine)
                    {
                        return feesReturnLine[src.Reference];
                    }

                    throw new InvalidOperationException("Return line fees are required.");
                }
            )
        );

        CreateMap<ReturnEstimated, ReturnEstimated>().ForMember(
            re => re.Fees,
            mce => mce.MapFrom(
                (_, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("feesReturn", out var item) && item is IEnumerable<string> feesReturn)
                    {
                        return feesReturn;
                    }

                    throw new InvalidOperationException("Return fees are required.");
                }
            )
        );

        CreateMap<ReturnLineEstimated, ReturnLineEstimated>().ForMember(
            rle => rle.Fees,
            mce => mce.MapFrom(
                (src, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("feesReturnLine", out var item) && item is ILookup<string, ReturnFeeEstimated> feesReturnLine)
                    {
                        return feesReturnLine[src.Reference];
                    }

                    throw new InvalidOperationException("Return line fees are required.");
                }
            )
        );

        CreateMap<ReturnFeeEstimated, ReturnFeeEstimated>().ForMember(
            rfe => rfe.Value,
            mce => mce.MapFrom(
                (src, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("value", out var item) && item is decimal value)
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
                (_, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("errorsReturn", out var item) && item is IEnumerable<string> errorsReturn)
                    {
                        return errorsReturn;
                    }

                    throw new InvalidOperationException("Return errors are required.");
                }
            )
        );

        CreateMap<ReturnLine, ReturnLineValidated>().ForMember(
            rl => rl.Messages,
            mce => mce.MapFrom(
                (src, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("errorsReturnLine", out var item) && item is ILookup<string, string> errorsReturnLine)
                    {
                        return errorsReturnLine[src.Reference];
                    }

                    throw new InvalidOperationException("Return line errors are required.");
                }
            )
        );
    }
}
