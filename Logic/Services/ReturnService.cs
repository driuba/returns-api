using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Returns.Domain.Dto;
using Returns.Domain.Enums;
using Returns.Domain.Services;
using Returns.Logic.Repositories;
using Returns.Logic.Utils;

namespace Returns.Logic.Services;

public class ReturnService : IReturnService
{
    private readonly ICustomerService _customerService;
    private readonly ReturnDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IRegionService _regionService;
    private readonly ISessionService _sessionService;
    private readonly IStorageService _storageService;

    public ReturnService(
        ICustomerService customerService,
        ReturnDbContext dbContext,
        IMapper mapper,
        IRegionService regionService,
        ISessionService sessionService,
        IStorageService storageService
    )
    {
        _customerService = customerService;
        _dbContext = dbContext;
        _mapper = mapper;
        _regionService = regionService;
        _sessionService = sessionService;
        _storageService = storageService;
    }

    public Task<ValueResponse<Domain.Entities.Return>> Create(Return returnCandidate)
    {
        throw new NotImplementedException();
    }

    public async Task<ValueResponse<Domain.Entities.Return>> Delete(int id)
    {
        var returnExisting = await _dbContext
            .Set<Domain.Entities.Return>()
            .AsTracking()
            .Include(r => r.Lines)
            .ThenInclude(l => l.Attachments)
            .SingleOrDefaultAsync(r => r.Id == id);

        if (returnExisting is null)
        {
            return new ValueResponse<Domain.Entities.Return>
            {
                Message = $"Return {id} was not found."
            };
        }

        if (returnExisting.State != ReturnState.New)
        {
            return new ValueResponse<Domain.Entities.Return>
            {
                Message = $"Only returns of state {ReturnState.New} can be deleted, current state: {returnExisting.State}."
            };
        }

        var storageIds = returnExisting.Lines
            .SelectMany(l => l.Attachments)
            .Select(a => a.StorageId)
            .Distinct()
            .ToList();

        if (storageIds.Any())
        {
            var response = await _storageService.Delete(storageIds);

            if (!response.Success)
            {
                return new ValueResponse<Domain.Entities.Return>
                {
                    Message = response.Message,
                    Messages = response.Messages
                };
            }
        }

        _dbContext
            .Set<Domain.Entities.Return>()
            .Remove(returnExisting);

        await _dbContext.SaveChangesAsync();

        return new ValueResponse<Domain.Entities.Return>
        {
            Success = true,
            Value = returnExisting
        };
    }

    public Task<ReturnEstimated> Estimate(Return returnCandidate)
    {
        throw new NotImplementedException();
    }

    public async Task<ValueResponse<Domain.Entities.Return>> Update(Domain.Entities.Return returnCandidate)
    {
        if (returnCandidate.LabelCount < 0)
        {
            return new ValueResponse<Domain.Entities.Return>
            {
                Message = "Label count must be a non-negative integer."
            };
        }

        var returnExisting = await _dbContext
            .Set<Domain.Entities.Return>()
            .AsTracking()
            .SingleOrDefaultAsync(r => r.Id == returnCandidate.Id);

        if (returnExisting is null)
        {
            return new ValueResponse<Domain.Entities.Return>
            {
                Message = $"Return {returnCandidate.Id} was not found."
            };
        }

        if (returnExisting.State != ReturnState.New)
        {
            return new ValueResponse<Domain.Entities.Return>
            {
                Message = $"Only returns of state {ReturnState.New} can be edited, current state: {returnExisting.State}."
            };
        }

        returnExisting.LabelCount = returnCandidate.LabelCount;

        await _dbContext.SaveChangesAsync();

        return new ValueResponse<Domain.Entities.Return>
        {
            Success = true,
            Value = returnExisting
        };
    }

    public async Task<ReturnValidated> Validate(Return returnCandidate)
    {
        var errorsReturn = new List<string>();

        var errorsReturnLine = returnCandidate.Lines
            .Select(rl => rl.Reference)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                r => r,
                _ => new List<string>(),
                StringComparer.OrdinalIgnoreCase
            );

        if (string.IsNullOrEmpty(returnCandidate.CustomerId))
        {
            errorsReturn.Add("Customer identifier is required.");
        }
        else if (
            string.IsNullOrEmpty(_sessionService.CustomerId) ||
            !string.Equals(returnCandidate.CustomerId, _sessionService.CustomerId, StringComparison.OrdinalIgnoreCase)
        )
        {
            errorsReturn.Add($"Customer {returnCandidate.CustomerId} is not valid for return.");
        }

        if (string.IsNullOrEmpty(returnCandidate.DeliveryPointId))
        {
            errorsReturn.Add("Delivery point identifier is required.");
        }

        if (returnCandidate.LabelCount < 0)
        {
            errorsReturn.Add("Label count must be a non-negative number.");
        }

        if (!returnCandidate.Lines.Any())
        {
            errorsReturn.Add("At least one return line is required.");
        }

        if (errorsReturn.Any())
        {
            return _mapper.Map<ReturnValidated>(returnCandidate);
        }

        var deliveryPoint = await _customerService.GetDeliveryPoint(returnCandidate.DeliveryPointId);

        if (deliveryPoint is null)
        {
            errorsReturn.Add($"Delivery point {returnCandidate.DeliveryPointId} was not found.");
        }
        else if (!string.Equals(returnCandidate.CustomerId, deliveryPoint.CustomerId, StringComparison.OrdinalIgnoreCase))
        {
            errorsReturn.Add($"Delivery point {returnCandidate.DeliveryPointId} does not belong to customer {returnCandidate.CustomerId}.");
        }

        if (errorsReturn.Any())
        {
            return _mapper.Map<ReturnValidated>(returnCandidate);
        }

        IEnumerable<ReturnLine> returnLines = returnCandidate.Lines.ToList();

        var referencesDuplicated = returnLines
            .Select(l => l.Reference)
            .GroupBy(r => r, StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        foreach (var referenceDuplicated in referencesDuplicated)
        {
            errorsReturnLine[referenceDuplicated].Add($"Return line reference {referenceDuplicated} has duplicates.");
        }

        returnLines = returnLines
            .ExceptBy(
                referencesDuplicated,
                rl => rl.Reference,
                StringComparer.OrdinalIgnoreCase
            )
            .ToList();

        if (!returnLines.Any())
        {
            return _mapper.Map<ReturnValidated>(returnCandidate);
        }

        var returnLineQuantities = returnLines.Select(l => (l.ProductType, l.Quantity));

        if (returnCandidate.Id.HasValue)
        {
            var returnLineIdsExcluded = returnLines
                .Where(rl => rl.Id.HasValue)
                .Select(rl => rl.Id)
                .Cast<int>();

            var returnExisting = await _dbContext
                .Set<Domain.Entities.Return>()
                .Where(r => r.Id == returnCandidate.Id)
                .Select(r => new
                {
                    Lines = r.Lines
                        .Where(l => returnLineIdsExcluded.Contains(l.Id))
                        .Select(l => new
                        {
                            l.ProductType,
                            l.Quantity
                        })
                })
                .SingleOrDefaultAsync();

            if (returnExisting is null)
            {
                errorsReturn.Add($"Customer {returnCandidate.CustomerId} return {returnCandidate.Id} was not found.");
            }
            else
            {
                returnLineQuantities = returnLineQuantities.Concat(
                    returnExisting.Lines.Select(l => (l.ProductType, l.Quantity))
                );
            }
        }

        returnLineQuantities = returnLineQuantities.ToList();

        if (returnLineQuantities.Any(rlq => rlq.ProductType == ReturnProductType.Serviced))
        {
            if (string.IsNullOrEmpty(returnCandidate.RmaNumber))
            {
                errorsReturn.Add("RMA number is required for serviced product return.");
            }

            if
            (
                returnLineQuantities.Count() > 1 ||
                returnLineQuantities.Single().Quantity != 1
            )
            {
                errorsReturn.Add("Only a single serviced product may be registered within a single return document.");
            }
        }
        else if (!string.IsNullOrEmpty(returnCandidate.RmaNumber))
        {
            errorsReturn.Add("RMA number is only allowed for serviced product returns.");
        }

        var serialNumbersDuplicated = returnLines
            .Where(rl => !string.IsNullOrEmpty(rl.SerialNumber))
            .Select(rl => rl.SerialNumber)
            .Cast<string>()
            .GroupBy(sn => sn, StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var returnLine in returnLines)
        {
            if (
                returnLine.ProductType != ReturnProductType.New &&
                (
                    returnLine.FeeConfigurationGroupIdDamagePackage.HasValue ||
                    returnLine.FeeConfigurationGroupIdDamageProduct.HasValue
                )
            )
            {
                errorsReturnLine[returnLine.Reference].Add(
                    $"Return line with product type {returnLine.ProductType} must not have damage levels defined."
                );
            }

            if (string.IsNullOrEmpty(returnLine.InvoiceNumber))
            {
                errorsReturnLine[returnLine.Reference].Add("Invoice number is required.");
            }

            if (string.IsNullOrEmpty(returnLine.ProductId))
            {
                errorsReturnLine[returnLine.Reference].Add("Product identifier is required.");
            }

            if (validateAttachments && !returnLine.Attachments.Any())
            {
                if (returnLine.ProductType == ReturnProductType.Defective)
                {
                    errorsReturnLine[returnLine.Reference].Add(
                        $"Return line with product type {returnLine.ProductType} must have a proof of defect attached."
                    );
                }
                else if (returnLine.ProductType == ReturnProductType.Serviced)
                {
                    errorsReturnLine[returnLine.Reference].Add(
                        $"Return line with product type {returnLine.ProductType} must have a service act attached."
                    );
                }
            }

            if (returnLine.Quantity <= 0)
            {
                errorsReturnLine[returnLine.Reference].Add("Quantity must be a positive integer.");
            }

            if (!string.IsNullOrEmpty(returnLine.SerialNumber))
            {
                if (returnLine.Quantity > 1)
                {
                    errorsReturnLine[returnLine.Reference].Add($"Product {returnLine.SerialNumber} must have quantity of one.");
                }

                if (serialNumbersDuplicated.Contains(returnLine.SerialNumber))
                {
                    errorsReturnLine[returnLine.Reference].Add($"Serial number {returnLine.SerialNumber} has duplicates.");
                }
            }
        }

        var feeConfigurationGroupIds = returnLines
            .Where(rl => rl.ProductType == ReturnProductType.New)
            .SelectMany(rl => new[]
            {
                rl.FeeConfigurationGroupIdDamagePackage,
                rl.FeeConfigurationGroupIdDamageProduct
            })
            .Where(fcgi => fcgi.HasValue)
            .Cast<int>()
            .Distinct()
            .ToList();

        if (feeConfigurationGroupIds.Any())
        {
            var feeConfigurationGroupsExisting = await _dbContext
                .Set<Domain.Entities.FeeConfigurationGroup>()
                .Where(fcg => feeConfigurationGroupIds.Contains(fcg.Id))
                .ToDictionaryAsync(fcg => fcg.Id);

            foreach (var returnLine in returnLines.Where(rl => rl.ProductType == ReturnProductType.New))
            {
                if (returnLine.FeeConfigurationGroupIdDamagePackage.HasValue)
                {
                    if (
                        feeConfigurationGroupsExisting.TryGetValue(
                            returnLine.FeeConfigurationGroupIdDamagePackage.Value,
                            out var feeConfigurationGroup
                        )
                    )
                    {
                        if (feeConfigurationGroup.Type != FeeConfigurationGroupType.DamagePackage)
                        {
                            errorsReturnLine[returnLine.Reference].Add(
                                $"Return line has been assigned an invalid package damage level of {feeConfigurationGroup.Id}."
                            );
                        }
                    }
                    else
                    {
                        errorsReturnLine[returnLine.Reference].Add(
                            $"Package damage level {returnLine.FeeConfigurationGroupIdDamagePackage} was not found."
                        );
                    }
                }

                if (returnLine.FeeConfigurationGroupIdDamageProduct.HasValue)
                {
                    if (
                        feeConfigurationGroupsExisting.TryGetValue(
                            returnLine.FeeConfigurationGroupIdDamageProduct.Value,
                            out var feeConfigurationGroup
                        )
                    )
                    {
                        if (feeConfigurationGroup.Type != FeeConfigurationGroupType.DamageProduct)
                        {
                            errorsReturnLine[returnLine.Reference].Add(
                                $"Return line has been assigned an invalid product damage level of {feeConfigurationGroup.Id}."
                            );
                        }
                    }
                    else
                    {
                        errorsReturnLine[returnLine.Reference].Add(
                            $"Product damage level {returnLine.FeeConfigurationGroupIdDamageProduct.Value} was not found."
                        );
                    }
                }
            }
        }

        returnLines = returnLines
            .Where(rl => !string.IsNullOrEmpty(rl.ProductId))
            .ToList();

        if (!returnLines.Any())
        {
            return _mapper.Map<ReturnValidated>(returnCandidate);
        }

        // TODO: product mocking
        var products = (
                await _productService.FilterAsync(
                    returnLines
                        .Select(rl => rl.ProductId)
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                )
            )
            .ToDictionary(i => i.ItemId, StringComparer.OrdinalIgnoreCase);

        foreach (var returnLine in returnLines.Where(rl => !products.ContainsKey(rl.ProductId)))
        {
            errorsReturnLine[returnLine.Reference].Add($"Product {returnLine.ProductId} was not found.");
        }

        returnLines = returnLines
            .Where(rl => products.ContainsKey(rl.ProductId))
            .ToList();

        if (!returnLines.Any())
        {
            return _mapper.Map<ReturnValidated>(returnCandidate);
        }

        foreach (var returnLine in returnLines.Where(rl => products[rl.ProductId].ByOrderOnly))
        {
            errorsReturnLine[returnLine.Reference].Add($"Product {returnLine.ProductId} was purchased by order and cannot be returned.");
        }

        returnLines = returnLines
            .Where(rl => !products[rl.ProductId].ByOrderOnly)
            .ToList();

        if (!returnLines.Any())
        {
            return _mapper.Map<ReturnValidated>(returnCandidate);
        }

        var returnLinesServiceable = returnLines
            .Where(rl => products[rl.ProductId].Serviceable)
            .Where(rl => rl.ProductType == ReturnProductType.UnderWarranty)
            .ToList();

        foreach (var returnLine in returnLinesServiceable)
        {
            errorsReturnLine[returnLine.Reference].Add($"Product {returnLine.ProductId} must be serviced and cannot be returned as an RMA product.");
        }

        returnLines = returnLines
            .Where(rl => !string.IsNullOrEmpty(rl.InvoiceNumber))
            .ToList();

        if (!returnLines.Any())
        {
            return _mapper.Map<ReturnValidated>(returnCandidate);
        }

        var invoiceNumbers = returnLines
            .Select(rl => rl.InvoiceNumber)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (invoiceNumbers.Any())
        {
            var returnLineIdsExcluded = returnLines
                .Where(rl => rl.Id.HasValue)
                .Select(rl => rl.Id)
                .Cast<int>()
                .ToList();

            var serialNumbers = returnLines
                .Where(rl => !string.IsNullOrEmpty(rl.SerialNumber))
                .Select(rl => rl.SerialNumber)
                .Cast<string>()
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (serialNumbers.Any())
            {
                var serialNumbersRegistered = await _dbContext
                    .Set<Domain.Entities.ReturnLineDevice>()
                    .Where(rld => !returnLineIdsExcluded.Contains(rld.Line.Id))
                    .Where(rld => invoiceNumbers.Contains(rld.Line.InvoiceNumberPurchase))
                    .Where(rld => serialNumbers.Contains(rld.SerialNumber))
                    .Select(rld => rld.SerialNumber)
                    .Distinct()
                    .ToHashSetAsync(StringComparer.OrdinalIgnoreCase);

                var returnLinesFiltered = returnLines.Where(rl =>
                    !string.IsNullOrEmpty(rl.SerialNumber) &&
                    serialNumbersRegistered.Contains(rl.SerialNumber)
                );

                foreach (var returnLine in returnLinesFiltered)
                {
                    errorsReturnLine[returnLine.Reference].Add($"Serial number {returnLine.SerialNumber} is already registered for return.");
                }
            }

            var returnLinesRegistered = await _dbContext
                .Set<Domain.Entities.ReturnLine>()
                .Where(rl => !returnLineIdsExcluded.Contains(rl.Id))
                .Where(rl => invoiceNumbers.Contains(rl.InvoiceNumberPurchase))
                .Where(rl => products.Keys.Contains(rl.ProductId))
                .GroupBy(rl => new { rl.InvoiceNumberPurchase, rl.ProductId })
                .Select(g => new
                {
                    g.Key.InvoiceNumberPurchase,
                    g.Key.ProductId,
                    Quantity = g.Sum(rl => rl.Quantity)
                })
                .ToListAsync();

            // TODO: fetch invoices and validate against them

            var comparer = new ValueTupleEqualityComparer<string, string>(StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase);

            var returnLinesMapped = returnLines
                .GroupBy(rl => (rl.InvoiceNumber, rl.ProductId), comparer)
                .GroupJoin(
                    returnLinesRegistered,
                    rlg => rlg.Key,
                    rld => (rld.InvoiceNumberPurchase, rld.ProductId),
                    (rlg, rldg) => new
                    {
                        InvoiceNumber = rlg.Key.Item1,
                        Lines = rlg,
                        ProductId = rlg.Key.Item2,
                        QuantityReturn = rlg.Sum(rl => rl.Quantity),
                        QuantityReturnRegistered = rldg.Sum(rlr => rlr.Quantity)
                    },
                    comparer
                )
                .GroupJoin(
                    invoiceLines,
                    rlg => (rlg.InvoiceNumber, rlg.ProductId),
                    il => (il.InvoiceNumber, il.ProductId),
                    (rlg, ilg) =>
                    {
                        var invoiceLineGroup = ilg.ToList();

                        return new
                        {
                            rlg.Lines,
                            rlg.QuantityReturn,
                            rlg.QuantityReturnRegistered,
                            InvoiceDate = invoiceLineGroup.FirstOrDefault()?.Created,
                            QuantityInvoice = invoiceLineGroup.Sum(l => l.Quantity),
                            SerialNumbersInvoice = invoiceLineGroup
                                .SelectMany(ilr => ilr.Serials)
                                .ToHashSet(StringComparer.OrdinalIgnoreCase)
                        };
                    },
                    comparer
                )
                .SelectMany(
                    rlg => rlg.Lines,
                    (rlg, rl) => new
                    {
                        rlg.InvoiceDate,
                        rlg.QuantityInvoice,
                        rlg.QuantityReturn,
                        rlg.QuantityReturnDrafted,
                        rlg.SerialNumbersInvoice,
                        Line = rl
                    }
                )
                .ToList();

            foreach (var returnLineMapped in returnLinesMapped)
            {
                if
                (
                    returnLineMapped.Line.Returned.HasValue &&
                    returnLineMapped.InvoiceDate.HasValue &&
                    returnLineMapped.Line.Returned.Value.Date < returnLineMapped.InvoiceDate.Value.Date
                )
                {
                    errorsReturnLine[returnLineMapped.Line.Reference].Add(
                        $"Return date ({returnLineMapped.Line.Returned:yyyy-MM-dd}) must either be empty or later than the invoice date ({returnLineMapped.InvoiceDate:yyyy-MM-dd})."
                    );
                }

                if (returnLineMapped.QuantityReturn > returnLineMapped.QuantityInvoice - returnLineMapped.QuantityReturnDrafted)
                {
                    if (returnLineMapped.QuantityInvoice == 0)
                    {
                        errorsReturnLine[returnLineMapped.Line.Reference].Add(
                            $"Invoice {returnLineMapped.Line.InvoiceNumber} product {returnLineMapped.Line.ProductId} is not available for return."
                        );
                    }
                    else if (returnLineMapped.QuantityInvoice == returnLineMapped.QuantityReturnDrafted)
                    {
                        errorsReturnLine[returnLineMapped.Line.Reference].Add(
                            $"All invoice {returnLineMapped.Line.InvoiceNumber} products {returnLineMapped.Line.ProductId} are already registered for return."
                        );
                    }
                    else
                    {
                        errorsReturnLine[returnLineMapped.Line.Reference].Add(
                            $"Return quantity is greater than available quantity ({returnLineMapped.QuantityInvoice - returnLineMapped.QuantityReturnDrafted})."
                        );
                    }
                }

                if
                (
                    !(
                        string.IsNullOrEmpty(returnLineMapped.Line.SerialNumber) ||
                        returnLineMapped.SerialNumbersInvoice.Contains(returnLineMapped.Line.SerialNumber!)
                    )
                )
                {
                    errorsReturnLine[returnLineMapped.Line.Reference].Add(
                        $"Serial number {returnLineMapped.Line.SerialNumber} for invoice {returnLineMapped.Line.InvoiceNumber} product {returnLineMapped.Line.ProductId} was not found."
                    );
                }
            }

            returnLinesMapped = returnLinesMapped
                .Where(rlm => rlm.Line.ProductType == ReturnProductType.New)
                .Where(rlm =>
                    rlm.InvoiceDate.HasValue &&
                    (
                        !rlm.Line.Returned.HasValue ||
                        rlm.Line.Returned.Value.Date >= rlm.InvoiceDate.Value.Date
                    )
                )
                .ToList();

            if (returnLinesMapped.Any())
            {
                var country = await _regionService.GetCountry(deliveryPoint!.CountryId);

                var countryId = country?.Id;
                var regionIds = country?.Regions.Select(r => r.Id) ?? Enumerable.Empty<int>();

                if (countryId.HasValue)
                {
                    regionIds = regionIds.Append(countryId.Value);
                }

                regionIds = regionIds.ToList();

                var returnAvailabilities = await _dbContext
                    .Set<Domain.Entities.ReturnAvailability>()
                    .Where(ra =>
                        !ra.CountryId.HasValue ||
                        regionIds.Contains(ra.CountryId.Value)
                    )
                    .ToDictionaryAsync(ra => ra.CountryId ?? default(int), ra => ra.Days);

                int availability;

                if (countryId.HasValue && returnAvailabilities.TryGetValue(countryId.Value, out var returnAvailability))
                {
                    availability = returnAvailability;
                }
                else
                {
                    regionIds = regionIds
                        .Intersect(returnAvailabilities.Keys)
                        .ToList();

                    if (regionIds.Any())
                    {
                        availability = returnAvailabilities[regionIds.First(ri => returnAvailabilities.ContainsKey(ri))];
                    }
                    else
                    {
                        availability = returnAvailabilities[default(int)];
                    }
                }

                returnLinesMapped = returnLinesMapped.Where(rlm =>
                    rlm.InvoiceDate.HasValue &&
                    ((rlm.Line.Returned?.Date ?? DateTime.Today) - rlm.InvoiceDate.Value.Date).Days > availability
                );

                foreach (var returnLineMapped in returnLinesMapped)
                {
                    errorsReturnLine[returnLineMapped.Line.Reference].Add(
                        $"Invoice {returnLineMapped.Line.InvoiceNumber} was created more than {availability} days ago ({((returnLineMapped.Line.Returned?.Date ?? DateTime.Today) - returnLineMapped.InvoiceDate!.Value.Date).Days}) and is not eligible for return."
                    );
                }
            }
        }

        return _mapper.Map<ReturnValidated>(returnCandidate);
    }
}
