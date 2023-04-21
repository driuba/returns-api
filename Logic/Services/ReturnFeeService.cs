using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Returns.Domain.Dto;
using Returns.Domain.Dto.Customers;
using Returns.Domain.Dto.Invoices;
using Returns.Domain.Dto.Regions;
using Returns.Domain.Enums;
using Returns.Domain.Services;
using Returns.Logic.Repositories;
using Returns.Logic.Utils;

namespace Returns.Logic.Services;

public class ReturnFeeService : IReturnFeeService
{
    private readonly ReturnDbContext _dbContext;
    private readonly IFeeConfigurationService _feeConfigurationService;
    private readonly IMapper _mapper;

    public ReturnFeeService(ReturnDbContext dbContext, IFeeConfigurationService feeConfigurationService, IMapper mapper)
    {
        _dbContext = dbContext;
        _feeConfigurationService = feeConfigurationService;
        _mapper = mapper;
    }

    public ReturnEstimated Calculate(ReturnEstimated returnEstimated, IEnumerable<InvoiceLine> invoiceLines)
    {
        var feesReturn = new List<ReturnFeeEstimated>();
        var feesReturnLine = new List<(string Reference, ReturnFeeEstimated Fee)>();

        var returnLines = returnEstimated.Lines
            .Where(l => l.Quantity > 0)
            .IntersectBy(
                returnEstimated.Lines
                    .Select(l => l.Reference)
                    .GroupBy(r => r, StringComparer.OrdinalIgnoreCase)
                    .Where(g => g.Count() == 1)
                    .Select(g => g.Key),
                l => l.Reference,
                StringComparer.OrdinalIgnoreCase
            )
            .GroupJoin(
                invoiceLines,
                rl => (rl.InvoiceNumber, rl.ProductId),
                il => (il.InvoiceNumber, il.ProductId),
                (rl, ilg) => new
                {
                    InvoiceLines = ilg,
                    ReturnLine = rl
                },
                new ValueTupleEqualityComparer<string, string>(StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase)
            )
            .Where(l => l.InvoiceLines.Any())
            .Select(l =>
            {
                var aggregate = l.InvoiceLines.Aggregate(
                    new
                    {
                        Price = 0M,
                        Quantity = 0
                    },
                    (acc, il) => new
                    {
                        Price = acc.Price + il.PriceUnit * il.Quantity,
                        Quantity = acc.Quantity + il.Quantity
                    }
                );

                return new
                {
                    Line = l.ReturnLine,
                    PriceUnit = aggregate.Price / aggregate.Quantity
                };
            })
            .ToList();

        if (!returnLines.Any())
        {
            return _mapper.Map<ReturnEstimated>(
                returnEstimated,
                moo =>
                {
                    moo.Items["feesReturn"] = feesReturn;
                    moo.Items["feesReturnLine"] = feesReturnLine.ToLookup(
                        frl => frl.Reference,
                        frl => frl.Fee,
                        StringComparer.OrdinalIgnoreCase
                    );
                }
            );
        }

        var returnLinesNew = returnLines
            .Where(rl => rl.Line.ProductType == ReturnProductType.New)
            .ToList();

        if (returnLinesNew.Any())
        {
            var priceTotal = returnLinesNew.Sum(rl => rl.PriceUnit * rl.Line.Quantity);

            feesReturn.AddRange(
                returnEstimated.Fees.Select(f => _mapper.Map<ReturnFeeEstimated>(
                        f,
                        moo =>
                        {
                            decimal value;

                            if (f.Configuration.ValueType == FeeValueType.Fixed)
                            {
                                value = f.Configuration.Value;
                            }
                            else
                            {
                                value = priceTotal * f.Configuration.Value;

                                if (f.Configuration.ValueMinimum.HasValue)
                                {
                                    value = Math.Max(value, f.Configuration.ValueMinimum.Value);
                                }
                            }

                            moo.Items["value"] = Math.Round(value, 2);
                        }
                    )
                )
            );
        }

        feesReturnLine.AddRange(
            returnLines.SelectMany(
                rl => rl.Line.Fees,
                (rl, f) =>
                (
                    rl.Line.Reference,
                    _mapper.Map<ReturnFeeEstimated>(
                        f,
                        moo =>
                        {
                            decimal value;

                            if (f.Configuration.ValueType == FeeValueType.Fixed)
                            {
                                value = f.Configuration.Value;
                            }
                            else
                            {
                                value = rl.PriceUnit * rl.Line.Quantity * f.Configuration.Value;

                                if (f.Configuration.ValueMinimum.HasValue)
                                {
                                    value = Math.Max(value, f.Configuration.ValueMinimum.Value);
                                }
                            }

                            moo.Items["value"] = Math.Round(value, 2);
                        }
                    )
                )
            )
        );

        return _mapper.Map<ReturnEstimated>(
            returnEstimated,
            moo =>
            {
                moo.Items["feesReturn"] = feesReturn;
                moo.Items["feesReturnLine"] = feesReturnLine.ToLookup(
                    frl => frl.Reference,
                    frl => frl.Fee,
                    StringComparer.OrdinalIgnoreCase
                );
            }
        );
    }

    public async Task<ReturnEstimated> Resolve(
        ReturnValidated returnValidated,
        Country? country,
        Customer deliveryPoint,
        IEnumerable<InvoiceLine> invoiceLines
    )
    {
        var feesReturn = new List<ReturnFeeEstimated>();
        var feesReturnLine = new List<(string Reference, ReturnFeeEstimated Fee)>();

        var returnLines = returnValidated.Lines
            .Where(l => l.Quantity > 0)
            .IntersectBy(
                returnValidated.Lines
                    .Select(l => l.Reference)
                    .GroupBy(r => r, StringComparer.OrdinalIgnoreCase)
                    .Where(g => g.Count() == 1)
                    .Select(g => g.Key),
                l => l.Reference,
                StringComparer.OrdinalIgnoreCase
            )
            .GroupJoin(
                invoiceLines,
                rl => (rl.InvoiceNumber, rl.ProductId),
                il => (il.InvoiceNumber, il.ProductId),
                (rl, ilg) => new
                {
                    ilg.FirstOrDefault()?.InvoiceDate,
                    Line = rl
                },
                new ValueTupleEqualityComparer<string, string>(StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase)
            )
            .ToList();

        if (!returnLines.Any(rl => rl.Line.ProductType == ReturnProductType.New || rl.Line.ApplyRegistrationFee))
        {
            return _mapper.Map<ReturnEstimated>(
                returnValidated,
                moo =>
                {
                    moo.Items["feesReturn"] = feesReturn;
                    moo.Items["feesReturnLine"] = feesReturnLine.ToLookup(
                        frl => frl.Reference,
                        frl => frl.Fee,
                        StringComparer.OrdinalIgnoreCase
                    );
                }
            );
        }

        var customerId = deliveryPoint.CustomerId;
        var countryId = country?.Id;
        var regionIds = country?.Regions.Select(r => r.Id) ?? Enumerable.Empty<int>();

        if (countryId.HasValue)
        {
            regionIds = regionIds.Append(countryId.Value);
        }

        regionIds = regionIds.ToList();

        var feeConfigurationGroupIds = returnLines
            .SelectMany(rl => new[]
            {
                rl.Line.FeeConfigurationGroupIdDamagePackage,
                rl.Line.FeeConfigurationGroupIdDamageProduct
            })
            .Where(fcgi => fcgi.HasValue)
            .Cast<int>()
            .Distinct();

        var feeConfigurations = await _dbContext
            .Set<Domain.Entities.FeeConfiguration>()
            .Include(fc => fc.Group)
            .Where(fc => !fc.Deleted)
            .Where(fc =>
                fc.Group.Type == FeeConfigurationGroupType.Administration ||
                fc.Group.Type == FeeConfigurationGroupType.Delay ||
                fc.Group.Type == FeeConfigurationGroupType.Registration ||
                (
                    (
                        fc.Group.Type == FeeConfigurationGroupType.DamagePackage ||
                        fc.Group.Type == FeeConfigurationGroupType.DamageProduct
                    ) &&
                    feeConfigurationGroupIds.Contains(fc.Group.Id)
                )
            )
            .Where(fc =>
                (
                    string.IsNullOrEmpty(fc.CustomerId) &&
                    !fc.RegionId.HasValue
                ) ||
                (
                    !string.IsNullOrEmpty(fc.CustomerId) &&
                    !fc.RegionId.HasValue &&
                    fc.CustomerId == customerId
                ) ||
                (
                    string.IsNullOrEmpty(fc.CustomerId) &&
                    fc.RegionId.HasValue &&
                    regionIds.Contains(fc.RegionId.Value)
                )
            )
            .ToListAsync();

        foreach (var returnLine in returnLines.Where(rl => rl.Line.ProductType == ReturnProductType.New))
        {
            if (returnLine.Line.FeeConfigurationGroupIdDamagePackage.HasValue)
            {
                var feeConfigurationDamage = _feeConfigurationService.Resolve(
                    feeConfigurations
                        .Where(f => f.Group.Id == returnLine.Line.FeeConfigurationGroupIdDamagePackage.Value)
                        .Where(fc => fc.Group.Type == FeeConfigurationGroupType.DamagePackage),
                    customerId,
                    countryId,
                    regionIds
                );

                if (feeConfigurationDamage is not null)
                {
                    feesReturnLine.Add((returnLine.Line.Reference, new ReturnFeeEstimated(feeConfigurationDamage)));
                }
            }

            if (returnLine.Line.FeeConfigurationGroupIdDamageProduct.HasValue)
            {
                var feeConfigurationDamage = _feeConfigurationService.Resolve(
                    feeConfigurations
                        .Where(f => f.Group.Id == returnLine.Line.FeeConfigurationGroupIdDamageProduct.Value)
                        .Where(fc => fc.Group.Type == FeeConfigurationGroupType.DamageProduct),
                    customerId,
                    countryId,
                    regionIds
                );

                if (feeConfigurationDamage is not null)
                {
                    feesReturnLine.Add((returnLine.Line.Reference, new ReturnFeeEstimated(feeConfigurationDamage)));
                }
            }

            // ReSharper disable once InvertIf
            if (returnLine.InvoiceDate.HasValue)
            {
                var feeConfigurationDelay = _feeConfigurationService.Resolve(
                    feeConfigurations
                        .Where(fc => fc.Group.Type == FeeConfigurationGroupType.Delay)
                        .GroupBy(fc => (returnLine.Line.Returned?.Date ?? DateTime.Today).AddDays(-fc.Group.DelayDays!.Value))
                        .Where(g => returnLine.InvoiceDate <= g.Key)
                        .MinBy(g => g.Key) ?? Enumerable.Empty<Domain.Entities.FeeConfiguration>(),
                    customerId,
                    countryId,
                    regionIds
                );

                if (feeConfigurationDelay is not null)
                {
                    feesReturnLine.Add(
                        (returnLine.Line.Reference,
                            new ReturnFeeEstimated(feeConfigurationDelay)
                            {
                                DelayDays = ((returnLine.Line.Returned?.Date ?? DateTime.Today) - returnLine.InvoiceDate.Value.Date).Days
                            }
                        )
                    );
                }
            }
        }

        var feeConfigurationRegistration = _feeConfigurationService.Resolve(
            feeConfigurations.Where(fc => fc.Group.Type == FeeConfigurationGroupType.Registration),
            customerId,
            countryId,
            regionIds
        );

        if (feeConfigurationRegistration is not null)
        {
            feesReturnLine.AddRange(
                returnLines
                    .Where(rl => rl.Line.ApplyRegistrationFee)
                    .Select(rl => (rl.Line.Reference, new ReturnFeeEstimated(feeConfigurationRegistration)))
            );
        }

        if (returnLines.Any(rl => rl.Line.ProductType == ReturnProductType.New))
        {
            feesReturn.AddRange(
                feeConfigurations
                    .Where(fc => fc.Group.Type == FeeConfigurationGroupType.Administration)
                    .GroupBy(fc => fc.Group.Id)
                    .Select(g => _feeConfigurationService.Resolve(g, customerId, countryId, regionIds))
                    .Where(fc => fc is not null)
                    .Cast<Domain.Entities.FeeConfiguration>()
                    .Select(fc => new ReturnFeeEstimated(fc))
            );
        }

        return _mapper.Map<ReturnEstimated>(
            returnValidated,
            moo =>
            {
                moo.Items["feesReturn"] = feesReturn;
                moo.Items["feesReturnLine"] = feesReturnLine.ToLookup(
                    frl => frl.Reference,
                    frl => frl.Fee,
                    StringComparer.OrdinalIgnoreCase
                );
            }
        );
    }
}
