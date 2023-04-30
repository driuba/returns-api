using Returns.Domain.Dto;

namespace Returns.Logic.Mappings;

public sealed class LogicProfile : AutoMapper.Profile
{
    public LogicProfile()
    {
        MapInvoiceLineReturnable();
        MapReturnEntity();
        MapReturnEstimated();
        MapReturnValidated();
    }

    private void MapInvoiceLineReturnable()
    {
        CreateMap<Domain.Dto.Invoices.InvoiceLine, InvoiceLineReturnable>()
            .ConstructUsing((src, ctx) =>
            {
                if (!(ctx.Items.TryGetValue("productName", out var item) && item is string productName))
                {
                    throw new InvalidOperationException("Product name is required.");
                }

                return new InvoiceLineReturnable(src.InvoiceNumber, src.ProductId, productName);
            })
            .ForMember(
                ilr => ilr.ByOrderOnly,
                mce => mce.MapFrom(
                    (_, _, _, ctx) =>
                    {
                        if (!(ctx.Items.TryGetValue("byOrderOnly", out var item) && item is bool byOrderOnly))
                        {
                            throw new InvalidOperationException("By order only flag is required.");
                        }

                        return byOrderOnly;
                    }
                )
            )
            .ForMember(
                ilr => ilr.QuantityInvoiced,
                mce => mce.MapFrom(src => src.Quantity)
            )
            .ForMember(
                ilr => ilr.QuantityReturned,
                mce => mce.MapFrom(
                    (_, _, _, ctx) =>
                    {
                        if (!(ctx.Items.TryGetValue("quantityReturned", out var item) && item is int quantityReturned))
                        {
                            throw new InvalidOperationException("Returned quantity is required.");
                        }

                        return quantityReturned;
                    }
                )
            )
            .ForMember(
                ilr => ilr.Serviceable,
                mce => mce.MapFrom(
                    (_, _, _, ctx) =>
                    {
                        if (!(ctx.Items.TryGetValue("serviceable", out var item) && item is bool serviceable))
                        {
                            throw new InvalidOperationException("Serviceability flag is required.");
                        }

                        return serviceable;
                    }
                )
            );
    }

    private void MapReturnEntity()
    {
        var returnMap = CreateMap<ReturnEstimated, Domain.Entities.Return>();

        returnMap.AfterMap((_, dest) =>
        {
            foreach (var fee in dest.Lines.SelectMany(l => l.Fees))
            {
                dest.Fees.Add(fee);
            }
        });

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
            rf => rf.Configuration,
            mce => mce.Ignore()
        );

        returnFeeMap.ForMember(
            rf => rf.FeeConfigurationId,
            mce => mce.MapFrom(src => src.Configuration.Id)
        );

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
                    if (ctx.Items.TryGetValue("feesReturn", out var item) && item is IEnumerable<ReturnFeeEstimated> feesReturn)
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

        var returnMap = CreateMap<ReturnEstimated, ReturnEstimated>();

        returnMap.ForMember(
            re => re.Fees,
            mce => mce.MapFrom(
                (_, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("feesReturn", out var item) && item is IEnumerable<ReturnFeeEstimated> feesReturn)
                    {
                        return feesReturn;
                    }

                    throw new InvalidOperationException("Return fees are required.");
                }
            )
        );

        returnMap.ForMember(
            re => re.Lines,
            mce => mce.MapFrom(
                (src, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("returnLines", out var item) && item is IEnumerable<ReturnLineEstimated> returnLines)
                    {
                        return returnLines;
                    }

                    return src.Lines;
                }
            )
        );

        var returnLineMap = CreateMap<ReturnLineEstimated, ReturnLineEstimated>();

        returnLineMap.ForMember(
            rle => rle.Fees,
            mce => mce.MapFrom(
                (src, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("feesReturnLine", out var item) && item is ILookup<string, ReturnFeeEstimated> feesReturnLine)
                    {
                        return feesReturnLine[src.Reference];
                    }

                    return src.Fees;
                }
            )
        );

        returnLineMap.ForMember(
            rle => rle.PriceUnit,
            mce => mce.MapFrom(
                (src, _, _, ctx) =>
                {
                    if (ctx.Items.TryGetValue("priceUnit", out var item) && item is decimal priceUnit)
                    {
                        return priceUnit;
                    }

                    return src.PriceUnit;
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
