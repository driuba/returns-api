using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Returns.Domain.Dto;
using Returns.Domain.Enums;
using Returns.Domain.Services;
using Returns.Logic.Repositories;
using Returns.Logic.Utils;

namespace Returns.Logic.Services;

public class ReturnLineService : IReturnLineService
{
    private readonly ICustomerService _customerService;
    private readonly ReturnDbContext _dbContext;
    private readonly IInvoiceService _invoiceService;
    private readonly IMapper _mapper;
    private readonly IRegionService _regionService;
    private readonly IReturnService _returnService;
    private readonly IReturnFeeService _returnFeeService;
    private readonly IStorageService _storageService;

    public ReturnLineService(
        ICustomerService customerService,
        ReturnDbContext dbContext,
        IInvoiceService invoiceService,
        IMapper mapper,
        IRegionService regionService,
        IReturnService returnService,
        IReturnFeeService returnFeeService,
        IStorageService storageService
    )
    {
        _customerService = customerService;
        _dbContext = dbContext;
        _invoiceService = invoiceService;
        _mapper = mapper;
        _regionService = regionService;
        _returnService = returnService;
        _returnFeeService = returnFeeService;
        _storageService = storageService;
    }

    public async Task<ValueResponse<Domain.Entities.ReturnLine>> CreateAsync(int returnId, ReturnLine returnLineCandidate)
    {
        var returnEntity = await _dbContext
            .Set<Domain.Entities.Return>()
            .Include(r => r.Lines)
            .ThenInclude(l => l.Fees)
            .ThenInclude(f => f.Configuration)
            .ThenInclude(c => c.Group)
            .SingleOrDefaultAsync(r => r.Id == returnId);

        if (returnEntity is null)
        {
            return new ValueResponse<Domain.Entities.ReturnLine>
            {
                Message = $"Return {returnId} was not found."
            };
        }

        if (returnEntity.State != ReturnState.New)
        {
            return new ValueResponse<Domain.Entities.ReturnLine>
            {
                Message = $"Only returns of state {ReturnState.New} can be edited, current state: {returnEntity.State}."
            };
        }

        var deliveryPoint = await _customerService.GetDeliveryPointAsync(returnEntity.DeliveryPointId);

        if (deliveryPoint is null)
        {
            return new ValueResponse<Domain.Entities.ReturnLine>
            {
                Message = $"Delivery point {returnEntity.DeliveryPointId} was not found."
            };
        }

        var country = await _regionService.GetCountryAsync(deliveryPoint.CountryId);

        var returnCandidate = _mapper.Map<Return>(returnEntity);

        returnCandidate.Lines = returnCandidate.Lines
            .Append(returnLineCandidate)
            .ToList();

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

        var returnValidated = await _returnService.ValidateAsync(
            returnCandidate,
            country,
            deliveryPoint,
            invoiceLines,
            validateAttachments: false
        );

        if (returnValidated.Messages.Any() || returnValidated.Lines.Any(l => l.Messages.Any()))
        {
            return new ValueResponse<Domain.Entities.ReturnLine>
            {
                Message = "One or more validation errors occured.",
                Messages = returnValidated.Messages.Union(
                    returnValidated.Lines.Select(l => $"Line {l.Reference}: {l.Messages}")
                )
            };
        }

        var returnEstimated = await _returnFeeService.ResolveAsync(returnValidated, country, invoiceLines);

        var response = await _returnService.MergeAsync(
            _returnFeeService.Calculate(returnEstimated, invoiceLines)
        );

        if (response is not { Success: true, Value: not null })
        {
            return new ValueResponse<Domain.Entities.ReturnLine>
            {
                Message = response.Message,
                Messages = response.Messages
            };
        }

        var returnLine = response.Value.Lines.Single(l => l.Id == 0);

        await _dbContext.SaveChangesAsync();

        return new ValueResponse<Domain.Entities.ReturnLine>
        {
            Success = true,
            Value = returnLine
        };
    }

    public async Task<ValueResponse<Domain.Entities.ReturnLine>> DeleteAsync(int returnId, int returnLineId)
    {
        var returnEntity = await _dbContext
            .Set<Domain.Entities.Return>()
            .Include(r => r.Lines)
            .ThenInclude(l => l.Attachments)
            .Include(r => r.Lines)
            .ThenInclude(l => l.Fees)
            .ThenInclude(f => f.Configuration)
            .ThenInclude(c => c.Group)
            .SingleOrDefaultAsync(r => r.Id == returnId);

        if (returnEntity is null)
        {
            return new ValueResponse<Domain.Entities.ReturnLine>
            {
                Message = $"Return {returnId} was not found."
            };
        }

        if (returnEntity.State != ReturnState.New)
        {
            return new ValueResponse<Domain.Entities.ReturnLine>
            {
                Message = $"Only returns of state {ReturnState.New} can be edited, current state: {returnEntity.State}."
            };
        }

        var returnLine = returnEntity.Lines.SingleOrDefault(l => l.Id == returnLineId);

        if (returnLine is null)
        {
            return new ValueResponse<Domain.Entities.ReturnLine>
            {
                Message = $"Return {returnId} line {returnLineId} was not found."
            };
        }

        var storageIds = returnLine.Attachments
            .Select(a => a.StorageId)
            .Distinct()
            .ToList();

        if (storageIds.Any())
        {
            var response = _storageService.Delete(storageIds);

            if (!response.Success)
            {
                return new ValueResponse<Domain.Entities.ReturnLine>
                {
                    Message = response.Message,
                    Messages = response.Messages
                };
            }
        }

        returnEntity.Lines.Remove(returnLine);

        if (returnEntity.Lines.Any())
        {
            var invoiceLines = await _invoiceService
                .FilterLinesAsync(
                    returnEntity.CustomerId,
                    returnEntity.Lines
                        .Select(l => l.InvoiceNumberPurchase)
                        .Distinct(StringComparer.OrdinalIgnoreCase),
                    returnEntity.Lines
                        .Select(l => l.ProductId)
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                )
                .ToListAsync();

            var returnEstimated = _mapper.Map<ReturnEstimated>(returnEntity);

            var response = await _returnService.MergeAsync(
                _returnFeeService.Calculate(returnEstimated, invoiceLines)
            );

            if (!response.Success)
            {
                return new ValueResponse<Domain.Entities.ReturnLine>
                {
                    Message = response.Message,
                    Messages = response.Messages
                };
            }
        }
        else
        {
            _dbContext
                .Set<Domain.Entities.Return>()
                .Remove(returnEntity);
        }

        await _dbContext.SaveChangesAsync();

        return new ValueResponse<Domain.Entities.ReturnLine>
        {
            Success = true,
            Value = returnLine
        };
    }

    public async Task<ValueResponse<Domain.Entities.ReturnLine>> UpdateAsync(int returnId, ReturnLine returnLineCandidate)
    {
        var returnEntity = await _dbContext
            .Set<Domain.Entities.Return>()
            .Include(r => r.Lines)
            .ThenInclude(l => l.Fees)
            .ThenInclude(f => f.Configuration)
            .ThenInclude(c => c.Group)
            .SingleOrDefaultAsync(r => r.Id == returnId);

        if (returnEntity is null)
        {
            return new ValueResponse<Domain.Entities.ReturnLine>
            {
                Message = $"Return {returnId} was not found."
            };
        }

        if (returnEntity.State != ReturnState.New)
        {
            return new ValueResponse<Domain.Entities.ReturnLine>
            {
                Message = $"Only returns of state {ReturnState.New} can be edited, current state: {returnEntity.State}."
            };
        }

        if (returnEntity.Lines.All(l => l.Id != returnLineCandidate.Id))
        {
            return new ValueResponse<Domain.Entities.ReturnLine>
            {
                Message = $"Return {returnId} line {returnLineCandidate.Id} was not found."
            };
        }

        var deliveryPoint = await _customerService.GetDeliveryPointAsync(returnEntity.DeliveryPointId);

        if (deliveryPoint is null)
        {
            return new ValueResponse<Domain.Entities.ReturnLine>
            {
                Message = $"Delivery point {returnEntity.DeliveryPointId} was not found."
            };
        }

        var country = await _regionService.GetCountryAsync(deliveryPoint.CountryId);

        var returnCandidate = _mapper.Map<Return>(returnEntity);

        returnCandidate.Lines = returnCandidate.Lines
            .Where(l => l.Id != returnLineCandidate.Id)
            .Append(returnLineCandidate)
            .ToList();

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

        var returnValidated = await _returnService.ValidateAsync(
            returnCandidate,
            country,
            deliveryPoint,
            invoiceLines,
            validateAttachments: false
        );

        if (returnValidated.Messages.Any() || returnValidated.Lines.Any(l => l.Messages.Any()))
        {
            return new ValueResponse<Domain.Entities.ReturnLine>
            {
                Message = "One or more validation errors occured.",
                Messages = returnValidated.Messages.Union(
                    returnValidated.Lines.Select(l => $"Line {l.Reference}: {l.Messages}")
                )
            };
        }

        var returnEstimated = await _returnFeeService.ResolveAsync(returnValidated, country, invoiceLines);

        var response = await _returnService.MergeAsync(
            _returnFeeService.Calculate(returnEstimated, invoiceLines)
        );

        if (response is not { Success: true, Value: not null })
        {
            return new ValueResponse<Domain.Entities.ReturnLine>
            {
                Message = response.Message,
                Messages = response.Messages
            };
        }

        var returnLine = response.Value.Lines.Single(l => l.Id == returnLineCandidate.Id);

        await _dbContext.SaveChangesAsync();

        return new ValueResponse<Domain.Entities.ReturnLine>
        {
            Success = true,
            Value = returnLine
        };
    }
}
