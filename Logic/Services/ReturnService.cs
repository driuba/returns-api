using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Returns.Domain.Dto;
using Returns.Domain.Dto.Customers;
using Returns.Domain.Dto.Invoices;
using Returns.Domain.Dto.Regions;
using Returns.Domain.Enums;
using Returns.Domain.Services;
using Returns.Logic.Repositories;
using Returns.Logic.Utils;

namespace Returns.Logic.Services;

public class ReturnService : IReturnService
{
    private readonly IConfiguration _configuration;
    private readonly ICustomerService _customerService;
    private readonly ReturnDbContext _dbContext;
    private readonly IInvoiceService _invoiceService;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IProductService _productService;
    private readonly IRegionService _regionService;
    private readonly IReturnFeeService _returnFeeService;
    private readonly ISessionService _sessionService;
    private readonly IStorageService _storageService;

    public ReturnService(
        IConfiguration configuration,
        ICustomerService customerService,
        ReturnDbContext dbContext,
        IInvoiceService invoiceService,
        // ReSharper disable once SuggestBaseTypeForParameterInConstructor
        ILogger<ReturnService> logger,
        IMapper mapper,
        IProductService productService,
        IRegionService regionService,
        IReturnFeeService returnFeeService,
        ISessionService sessionService,
        IStorageService storageService
    )
    {
        _configuration = configuration;
        _customerService = customerService;
        _dbContext = dbContext;
        _invoiceService = invoiceService;
        _logger = logger;
        _mapper = mapper;
        _productService = productService;
        _regionService = regionService;
        _returnFeeService = returnFeeService;
        _sessionService = sessionService;
        _storageService = storageService;
    }

    public async Task<ValueResponse<Domain.Entities.Return>> CreateAsync(Return returnCandidate)
    {
        var deliveryPoint = await _customerService.GetDeliveryPointAsync(returnCandidate.DeliveryPointId);

        if (deliveryPoint is null)
        {
            return new ValueResponse<Domain.Entities.Return>
            {
                Message = $"Delivery point {returnCandidate.DeliveryPointId} was not found."
            };
        }

        var country = await _regionService.GetCountryAsync(deliveryPoint.CountryId);

        var invoiceLines = await _invoiceService
            .FilterLinesAsync(
                returnCandidate.CustomerId,
                returnCandidate.Lines
                    .Select(l => l.InvoiceNumber)
                    .Distinct(StringComparer.OrdinalIgnoreCase),
                returnCandidate.Lines
                    .Select(l => l.ProductId)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
            )
            .ToListAsync();

        var returnValidated = await ValidateAsync(
            returnCandidate,
            country,
            deliveryPoint,
            invoiceLines,
            validateAttachments: false
        );

        if (returnValidated.Messages.Any() || returnValidated.Lines.Any(l => l.Messages.Any()))
        {
            return new ValueResponse<Domain.Entities.Return>
            {
                Message = "One or more validation errors occured.",
                Messages = returnValidated.Messages.Union(
                    returnValidated.Lines.Select(l => $"Line {l.Reference}: {l.Messages}")
                )
            };
        }

        var returnEstimated = await _returnFeeService.ResolveAsync(
            returnValidated,
            country,
            invoiceLines
        );

        var returnEntity = _mapper.Map<Domain.Entities.Return>(
            _returnFeeService.Calculate(returnEstimated, invoiceLines)
        );

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            _dbContext
                .Set<Domain.Entities.Return>()
                .Add(returnEntity);

            await _dbContext.SaveChangesAsync();

            var prefix = _configuration["ReturnNumberPrefix"] ?? string.Empty;

            returnEntity.Number = $"{prefix}{returnEntity.Id.ToString($"D{10 - prefix.Length}")}";

            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to save the return.");

            await transaction.RollbackAsync();

            return new ValueResponse<Domain.Entities.Return>
            {
                Message = "Failed to save the return."
            };
        }

        return new ValueResponse<Domain.Entities.Return>
        {
            Success = true,
            Value = returnEntity
        };
    }

    public async Task<ValueResponse<Domain.Entities.Return>> DeleteAsync(int id)
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
            var response = _storageService.Delete(storageIds);

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

    public async Task<ValueResponse<ReturnEstimated>> EstimateAsync(Return returnCandidate)
    {
        var deliveryPoint = await _customerService.GetDeliveryPointAsync(returnCandidate.DeliveryPointId);

        if (deliveryPoint is null)
        {
            return new ValueResponse<ReturnEstimated>
            {
                Message = $"Delivery point {returnCandidate.DeliveryPointId} was not found."
            };
        }

        var country = await _regionService.GetCountryAsync(deliveryPoint.CountryId);

        var invoiceLines = await _invoiceService
            .FilterLinesAsync(
                returnCandidate.CustomerId,
                returnCandidate.Lines
                    .Select(l => l.InvoiceNumber)
                    .Distinct(StringComparer.OrdinalIgnoreCase),
                returnCandidate.Lines
                    .Select(l => l.ProductId)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
            )
            .ToListAsync();

        var returnValidated = await ValidateAsync(
            returnCandidate,
            country,
            deliveryPoint,
            invoiceLines,
            validateAttachments: false
        );

        var returnEstimated = await _returnFeeService.ResolveAsync(returnValidated, country, invoiceLines);

        return new ValueResponse<ReturnEstimated>
        {
            Success = true,
            Value = _returnFeeService.Calculate(returnEstimated, invoiceLines)
        };
    }

    public Task<ValueResponse<Domain.Entities.Return>> MergeAsync(ReturnEstimated returnCandidate)
    {
        throw new NotImplementedException();
    }

    public async Task<ValueResponse<Domain.Entities.Return>> UpdateAsync(Domain.Entities.Return returnCandidate)
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

    public async Task<ReturnValidated> ValidateAsync(
        Return returnCandidate,
        Country? country,
        Customer deliveryPoint,
        IEnumerable<InvoiceLine> invoiceLines,
        bool validateAttachments
    )
    {
        var errorsReturn = new List<string>();
        var errorsReturnLine = new List<(string Reference, string Message)>();

        if (string.IsNullOrEmpty(returnCandidate.CustomerId))
        {
            errorsReturn.Add("Customer identifier is required.");
        }
        else if (
            !(
                string.IsNullOrEmpty(_sessionService.CustomerId) ||
                string.Equals(returnCandidate.CustomerId, _sessionService.CustomerId, StringComparison.OrdinalIgnoreCase)
            )
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
            return _mapper.Map<ReturnValidated>(
                returnCandidate,
                moo =>
                {
                    moo.Items["errorsReturn"] = errorsReturn;
                    moo.Items["errorsReturnLine"] = errorsReturnLine.ToLookup(
                        erl => erl.Reference,
                        erl => erl.Message,
                        StringComparer.OrdinalIgnoreCase
                    );
                }
            );
        }

        if (!string.Equals(returnCandidate.CustomerId, deliveryPoint.CustomerId, StringComparison.OrdinalIgnoreCase))
        {
            errorsReturn.Add($"Delivery point {returnCandidate.DeliveryPointId} does not belong to customer {returnCandidate.CustomerId}.");
        }

        if (errorsReturn.Any())
        {
            return _mapper.Map<ReturnValidated>(
                returnCandidate,
                moo =>
                {
                    moo.Items["errorsReturn"] = errorsReturn;
                    moo.Items["errorsReturnLine"] = errorsReturnLine.ToLookup(
                        erl => erl.Reference,
                        erl => erl.Message,
                        StringComparer.OrdinalIgnoreCase
                    );
                }
            );
        }

        IEnumerable<ReturnLine> returnLines = returnCandidate.Lines.ToList();

        var referencesDuplicated = returnLines
            .Select(l => l.Reference)
            .GroupBy(r => r, StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        errorsReturnLine.AddRange(
            referencesDuplicated.Select(rd => (rd, $"Return line reference {rd} has duplicates."))
        );

        returnLines = returnLines
            .ExceptBy(
                referencesDuplicated,
                rl => rl.Reference,
                StringComparer.OrdinalIgnoreCase
            )
            .ToList();

        if (!returnLines.Any())
        {
            return _mapper.Map<ReturnValidated>(
                returnCandidate,
                moo =>
                {
                    moo.Items["errorsReturn"] = errorsReturn;
                    moo.Items["errorsReturnLine"] = errorsReturnLine.ToLookup(
                        erl => erl.Reference,
                        erl => erl.Message,
                        StringComparer.OrdinalIgnoreCase
                    );
                }
            );
        }

        var returnLineQuantities = returnLines.Select(l => (l.ProductType, l.Quantity));

        var returnLineIdsExcluded = returnLines
            .Where(rl => rl.Id.HasValue)
            .Select(rl => rl.Id)
            .Cast<int>();

        if (returnCandidate.Id.HasValue)
        {
            var returnExisting = await _dbContext
                .Set<Domain.Entities.Return>()
                .Where(r => r.Id == returnCandidate.Id)
                .Select(r => new
                {
                    Lines = r.Lines
                        // ReSharper disable once AccessToModifiedClosure
                        .Where(l => !returnLineIdsExcluded.Contains(l.Id))
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
                errorsReturnLine.Add(
                    (
                        returnLine.Reference,
                        $"Return line with product type {returnLine.ProductType} must not have damage levels defined."
                    )
                );
            }

            if (string.IsNullOrEmpty(returnLine.InvoiceNumber))
            {
                errorsReturnLine.Add((returnLine.Reference, "Invoice number is required."));
            }

            if (string.IsNullOrEmpty(returnLine.ProductId))
            {
                errorsReturnLine.Add((returnLine.Reference, "Product identifier is required."));
            }

            if (validateAttachments && !returnLine.Attachments.Any())
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (returnLine.ProductType == ReturnProductType.Defective)
                {
                    errorsReturnLine.Add(
                        (
                            returnLine.Reference,
                            $"Return line with product type {returnLine.ProductType} must have a proof of defect attached."
                        )
                    );
                }
                else if (returnLine.ProductType == ReturnProductType.Serviced)
                {
                    errorsReturnLine.Add(
                        (
                            returnLine.Reference,
                            $"Return line with product type {returnLine.ProductType} must have a service act attached."
                        )
                    );
                }
            }

            if (returnLine.Quantity <= 0)
            {
                errorsReturnLine.Add((returnLine.Reference, "Quantity must be a positive integer."));
            }

            // ReSharper disable once InvertIf
            if (!string.IsNullOrEmpty(returnLine.SerialNumber))
            {
                if (returnLine.Quantity > 1)
                {
                    errorsReturnLine.Add((returnLine.Reference, $"Product {returnLine.SerialNumber} must have quantity of one."));
                }

                if (serialNumbersDuplicated.Contains(returnLine.SerialNumber))
                {
                    errorsReturnLine.Add((returnLine.Reference, $"Serial number {returnLine.SerialNumber} has duplicates."));
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
                            errorsReturnLine.Add(
                                (
                                    returnLine.Reference,
                                    $"Return line has been assigned an invalid package damage level of {feeConfigurationGroup.Id}."
                                )
                            );
                        }
                    }
                    else
                    {
                        errorsReturnLine.Add(
                            (
                                returnLine.Reference,
                                $"Package damage level {returnLine.FeeConfigurationGroupIdDamagePackage} was not found."
                            )
                        );
                    }
                }

                // ReSharper disable once InvertIf
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
                            errorsReturnLine.Add(
                                (
                                    returnLine.Reference,
                                    $"Return line has been assigned an invalid product damage level of {feeConfigurationGroup.Id}."
                                )
                            );
                        }
                    }
                    else
                    {
                        errorsReturnLine.Add(
                            (
                                returnLine.Reference,
                                $"Product damage level {returnLine.FeeConfigurationGroupIdDamageProduct.Value} was not found."
                            )
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
            return _mapper.Map<ReturnValidated>(
                returnCandidate,
                moo =>
                {
                    moo.Items["errorsReturn"] = errorsReturn;
                    moo.Items["errorsReturnLine"] = errorsReturnLine.ToLookup(
                        erl => erl.Reference,
                        erl => erl.Message,
                        StringComparer.OrdinalIgnoreCase
                    );
                }
            );
        }

        var products = await _productService
            .FilterAsync(
                returnLines
                    .Select(rl => rl.ProductId)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
            )
            .ToListAsync();

        errorsReturnLine.AddRange(
            returnLines
                .ExceptBy(
                    products.Select(p => p.Id),
                    rl => rl.ProductId,
                    StringComparer.OrdinalIgnoreCase
                )
                .Select(rl => (rl.Reference, $"Product {rl.ProductId} was not found."))
        );

        var returnLinesProduct = returnLines
            .Join(
                products,
                rl => rl.ProductId,
                p => p.Id,
                (rl, p) => new
                {
                    Product = p,
                    Line = rl
                },
                StringComparer.OrdinalIgnoreCase
            )
            .ToList();

        if (!returnLinesProduct.Any())
        {
            return _mapper.Map<ReturnValidated>(
                returnCandidate,
                moo =>
                {
                    moo.Items["errorsReturn"] = errorsReturn;
                    moo.Items["errorsReturnLine"] = errorsReturnLine.ToLookup(
                        erl => erl.Reference,
                        erl => erl.Message,
                        StringComparer.OrdinalIgnoreCase
                    );
                }
            );
        }

        foreach (var returnLineProduct in returnLinesProduct)
        {
            if (returnLineProduct.Product.ByOrderOnly)
            {
                errorsReturnLine.Add(
                    (
                        returnLineProduct.Line.Reference,
                        $"Product {returnLineProduct.Product.Id} was purchased by order and cannot be returned."
                    )
                );
            }

            if (returnLineProduct.Line.ProductType == ReturnProductType.UnderWarranty && returnLineProduct.Product.Serviceable)
            {
                errorsReturnLine.Add(
                    (
                        returnLineProduct.Line.Reference,
                        $"Product {returnLineProduct.Product.Id} must be serviced and cannot be returned under warranty."
                    )
                );
            }
        }

        returnLines = returnLinesProduct
            .Where(rlp => !rlp.Product.ByOrderOnly)
            .Where(rlp => !string.IsNullOrEmpty(rlp.Line.InvoiceNumber))
            .Select(rlp => rlp.Line)
            .ToList();

        if (!returnLines.Any())
        {
            return _mapper.Map<ReturnValidated>(
                returnCandidate,
                moo =>
                {
                    moo.Items["errorsReturn"] = errorsReturn;
                    moo.Items["errorsReturnLine"] = errorsReturnLine.ToLookup(
                        erl => erl.Reference,
                        erl => erl.Message,
                        StringComparer.OrdinalIgnoreCase
                    );
                }
            );
        }

        var invoiceNumbers = returnLines
            .Select(rl => rl.InvoiceNumber)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var productIds = returnLines
            .Select(rl => rl.ProductId)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        returnLineIdsExcluded = returnLines
            .Where(rl => rl.Id.HasValue)
            .Select(rl => rl.Id)
            .Cast<int>()
            .ToList();

        var serialNumbers = returnLines
            .Select(rl => rl.SerialNumber)
            .Where(sn => !string.IsNullOrEmpty(sn))
            .Cast<string>()
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (serialNumbers.Any())
        {
            var serialNumbersRegistered = await _dbContext
                .Set<Domain.Entities.ReturnLine>()
                .Where(rl => !returnLineIdsExcluded.Contains(rl.Id))
                .Where(rl => invoiceNumbers.Contains(rl.InvoiceNumberPurchase))
                .Where(rl => productIds.Contains(rl.ProductId))
                .Where(rl =>
                    !string.IsNullOrEmpty(rl.SerialNumber) &&
                    serialNumbers.Contains(rl.SerialNumber)
                )
                .Select(rl => rl.SerialNumber)
                .Distinct()
                .Cast<string>()
                .ToHashSetAsync(StringComparer.OrdinalIgnoreCase);

            errorsReturnLine.AddRange(
                returnLines
                    .Where(rl =>
                        !string.IsNullOrEmpty(rl.SerialNumber) &&
                        serialNumbersRegistered.Contains(rl.SerialNumber)
                    )
                    .Select(rl => (rl.Reference, $"Serial number {rl.SerialNumber} is already registered for return.")));
        }

        var returnLinesRegistered = await _dbContext
            .Set<Domain.Entities.ReturnLine>()
            .Where(rl => !returnLineIdsExcluded.Contains(rl.Id))
            .Where(rl => invoiceNumbers.Contains(rl.InvoiceNumberPurchase))
            .Where(rl => productIds.Contains(rl.ProductId))
            .GroupBy(rl => new
            {
                rl.InvoiceNumberPurchase,
                rl.ProductId
            })
            .Select(g => new
            {
                g.Key.InvoiceNumberPurchase,
                g.Key.ProductId,
                Quantity = g.Sum(rl => rl.Quantity)
            })
            .ToListAsync();

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
                        invoiceLineGroup.FirstOrDefault()?.InvoiceDate,
                        rlg.Lines,
                        rlg.QuantityReturn,
                        rlg.QuantityReturnRegistered,
                        QuantityInvoice = invoiceLineGroup.Sum(l => l.Quantity),
                        SerialNumbersInvoice = invoiceLineGroup
                            .Select(il => il.SerialNumber)
                            .Where(sn => !string.IsNullOrEmpty(sn))
                            .Cast<string>()
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
                    rlg.QuantityReturnRegistered,
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
                errorsReturnLine.Add(
                    (
                        returnLineMapped.Line.Reference,
                        $"Return date ({returnLineMapped.Line.Returned:yyyy-MM-dd}) must either be empty or later than the invoice date ({returnLineMapped.InvoiceDate:yyyy-MM-dd})."
                    )
                );
            }

            if (returnLineMapped.QuantityReturn > returnLineMapped.QuantityInvoice - returnLineMapped.QuantityReturnRegistered)
            {
                if (returnLineMapped.QuantityInvoice == 0)
                {
                    errorsReturnLine.Add(
                        (
                            returnLineMapped.Line.Reference,
                            $"Invoice {returnLineMapped.Line.InvoiceNumber} product {returnLineMapped.Line.ProductId} is not available for return."
                        )
                    );
                }
                else if (returnLineMapped.QuantityInvoice == returnLineMapped.QuantityReturnRegistered)
                {
                    errorsReturnLine.Add(
                        (
                            returnLineMapped.Line.Reference,
                            $"All invoice {returnLineMapped.Line.InvoiceNumber} products {returnLineMapped.Line.ProductId} are already registered for return."
                        )
                    );
                }
                else
                {
                    errorsReturnLine.Add(
                        (
                            returnLineMapped.Line.Reference,
                            $"Return quantity is greater than available quantity ({returnLineMapped.QuantityInvoice - returnLineMapped.QuantityReturnRegistered})."
                        )
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
                errorsReturnLine.Add(
                    (
                        returnLineMapped.Line.Reference,
                        $"Serial number {returnLineMapped.Line.SerialNumber} for invoice {returnLineMapped.Line.InvoiceNumber} product {returnLineMapped.Line.ProductId} was not found."
                    )
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

        if (!returnLinesMapped.Any())
        {
            return _mapper.Map<ReturnValidated>(
                returnCandidate,
                moo =>
                {
                    moo.Items["errorsReturn"] = errorsReturn;
                    moo.Items["errorsReturnLine"] = errorsReturnLine.ToLookup(
                        erl => erl.Reference,
                        erl => erl.Message,
                        StringComparer.OrdinalIgnoreCase
                    );
                }
            );
        }

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
                !ra.RegionId.HasValue ||
                // ReSharper disable once AccessToModifiedClosure
                regionIds.Contains(ra.RegionId.Value)
            )
            .ToDictionaryAsync(ra => ra.RegionId ?? default(int), ra => ra.Days);

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

            availability = regionIds.Any()
                ? returnAvailabilities[regionIds.First(ri => returnAvailabilities.ContainsKey(ri))]
                : returnAvailabilities[default(int)];
        }

        errorsReturnLine.AddRange(
            returnLinesMapped
                .Where(rlm =>
                    rlm.InvoiceDate.HasValue &&
                    ((rlm.Line.Returned?.Date ?? DateTime.Today) - rlm.InvoiceDate.Value.Date).Days > availability
                )
                .Select(rlm => (
                    rlm.Line.Reference,
                    $"Invoice {rlm.Line.InvoiceNumber} was created more than {availability} days ago ({((rlm.Line.Returned?.Date ?? DateTime.Today) - rlm.InvoiceDate!.Value.Date).Days}) and is not eligible for return."
                ))
        );

        return _mapper.Map<ReturnValidated>(
            returnCandidate,
            moo =>
            {
                moo.Items["errorsReturn"] = errorsReturn;
                moo.Items["errorsReturnLine"] = errorsReturnLine.ToLookup(
                    erl => erl.Reference,
                    erl => erl.Message,
                    StringComparer.OrdinalIgnoreCase
                );
            }
        );
    }
}
