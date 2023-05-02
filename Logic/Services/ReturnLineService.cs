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

    public async Task<ValueResponse<IEnumerable<Domain.Entities.ReturnLine>>> CreateAsync(int returnId, IEnumerable<ReturnLine> returnLinesCandidate)
    {
        returnLinesCandidate = returnLinesCandidate.ToList();

        if (!returnLinesCandidate.Any())
        {
            return new ValueResponse<IEnumerable<Domain.Entities.ReturnLine>> { Message = "At least one return line is required." };
        }

        var returnEntity = await _dbContext
            .Set<Domain.Entities.Return>()
            .Include(r => r.Lines)
            .ThenInclude(l => l.Fees)
            .ThenInclude(f => f.Configuration)
            .ThenInclude(c => c.Group)
            .SingleOrDefaultAsync(r => r.Id == returnId);

        if (returnEntity is null)
        {
            return new ValueResponse<IEnumerable<Domain.Entities.ReturnLine>> { Message = $"Return {returnId} was not found." };
        }

        if (returnEntity.State != ReturnState.New)
        {
            return new ValueResponse<IEnumerable<Domain.Entities.ReturnLine>>
            {
                Message = $"Only returns of state {ReturnState.New} can be edited, current state: {returnEntity.State}."
            };
        }

        var deliveryPoint = await _customerService.GetDeliveryPointAsync(returnEntity.DeliveryPointId);

        if (deliveryPoint is null)
        {
            return new ValueResponse<IEnumerable<Domain.Entities.ReturnLine>>
            {
                Message = $"Delivery point {returnEntity.DeliveryPointId} was not found."
            };
        }

        var country = await _regionService.GetCountryAsync(deliveryPoint.CountryId);

        var returnCandidate = _mapper.Map<Return>(returnEntity);

        returnCandidate.Lines = returnCandidate.Lines
            .Concat(returnLinesCandidate)
            .ToList();

        var invoiceLines = await _invoiceService
            .FilterLinesAsync(
                deliveryPoint.CustomerId,
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
            return new ValueResponse<IEnumerable<Domain.Entities.ReturnLine>>
            {
                Message = "One or more validation errors occured.",
                Messages = returnValidated.Messages.Union(
                    returnValidated.Lines.SelectMany(
                        l => l.Messages,
                        (l, m) => $"Line {l.Reference}: {m}"
                    )
                )
            };
        }

        var returnEstimated = await _returnFeeService.ResolveAsync(returnValidated, deliveryPoint, country, invoiceLines);

        var response = await _returnService.MergeAsync(
            _returnFeeService.Calculate(returnEstimated, invoiceLines)
        );

        if (response is not { Success: true, Value: not null })
        {
            return new ValueResponse<IEnumerable<Domain.Entities.ReturnLine>> { Message = response.Message, Messages = response.Messages };
        }

        var returnLines = response.Value.Lines.Where(l => l.Id == 0);

        await _dbContext.SaveChangesAsync();

        return new ValueResponse<IEnumerable<Domain.Entities.ReturnLine>> { Success = true, Value = returnLines };
    }

    public async Task<Response> DeclineAsync(int returnId, IEnumerable<int> returnLineIds, string note)
    {
        returnLineIds = returnLineIds.ToList();

        if (!returnLineIds.Any())
        {
            return new Response { Message = "At least one return line identifier is required." };
        }

        var returnEntity = await _dbContext
            .Set<Domain.Entities.Return>()
            .AsTracking()
            .Include(r => r.Lines)
            .SingleOrDefaultAsync(r => r.Id == returnId);

        if (returnEntity is null)
        {
            return new Response { Message = $"Return {returnId} was not found." };
        }

        if (returnEntity.State != ReturnState.Registered)
        {
            return new Response { Message = $"Only returns of state {ReturnState.Registered} can be edited, current state: {returnEntity.State}." };
        }

        var returnLineIdsMissing = returnLineIds
            .Except(
                returnEntity.Lines.Select(l => l.Id)
            )
            .ToList();

        if (returnLineIdsMissing.Any())
        {
            return new Response
            {
                Message = "One or more return lines were not found.",
                Messages = returnLineIdsMissing.Select(rli => $"Return line {rli} was not found.")
            };
        }

        var returnLines = returnEntity.Lines
            .IntersectBy(returnLineIds, l => l.Id)
            .ToList();

        if (returnLines.Any(rl => rl.State != ReturnLineState.Registered))
        {
            return new Response
            {
                Message = "One or more lines are in invalid state.",
                Messages = returnLines
                    .Where(rl => rl.State != ReturnLineState.Registered)
                    .Select(rl =>
                        $"Return line {rl.Id} state {rl.State} is not valid for declining, return line must be in {ReturnLineState.Registered} state."
                    )
            };
        }

        foreach (var returnLine in returnLines)
        {
            returnLine.NoteResponse = note;
            returnLine.State = ReturnLineState.Declined;
        }

        if (returnEntity.Lines.All(rl => rl.State != ReturnLineState.Registered))
        {
            if (returnEntity.Lines.All(rl => rl.State == ReturnLineState.Declined))
            {
                returnEntity.State = ReturnState.Declined;
            }
            else if (returnEntity.Lines.All(rl => rl.State == ReturnLineState.Invoiced))
            {
                returnEntity.State = ReturnState.Invoiced;
            }
            else
            {
                returnEntity.State = ReturnState.InvoicedPartially;
            }
        }

        await _dbContext.SaveChangesAsync();

        return new Response { Success = true };
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
            return new ValueResponse<Domain.Entities.ReturnLine> { Message = $"Return {returnId} was not found." };
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
            return new ValueResponse<Domain.Entities.ReturnLine> { Message = $"Return {returnId} line {returnLineId} was not found." };
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
                return new ValueResponse<Domain.Entities.ReturnLine> { Message = response.Message, Messages = response.Messages };
            }
        }

        returnEntity.Lines.Remove(returnLine);

        if (returnEntity.Lines.Any())
        {
            var deliveryPoint = await _customerService.GetDeliveryPointAsync(returnEntity.DeliveryPointId);

            if (deliveryPoint is null)
            {
                return new ValueResponse<Domain.Entities.ReturnLine> { Message = $"Delivery point {returnEntity.DeliveryPointId} was not found." };
            }

            var country = await _regionService.GetCountryAsync(deliveryPoint.CountryId);

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

            var returnEstimated = await _returnFeeService.ResolveAsync(
                _mapper.Map<ReturnValidated>(returnEntity),
                deliveryPoint,
                country,
                invoiceLines
            );

            var response = await _returnService.MergeAsync(
                _returnFeeService.Calculate(returnEstimated, invoiceLines)
            );

            if (!response.Success)
            {
                return new ValueResponse<Domain.Entities.ReturnLine> { Message = response.Message, Messages = response.Messages };
            }
        }
        else
        {
            _dbContext
                .Set<Domain.Entities.Return>()
                .Remove(returnEntity);
        }

        await _dbContext.SaveChangesAsync();

        return new ValueResponse<Domain.Entities.ReturnLine> { Success = true, Value = returnLine };
    }

    public async Task<Response> InvoiceAsync(int returnId, IEnumerable<int> returnLineIds)
    {
        returnLineIds = returnLineIds.ToList();

        if (!returnLineIds.Any())
        {
            return new Response { Message = "At least one return line identifier is required." };
        }

        var returnEntity = await _dbContext
            .Set<Domain.Entities.Return>()
            .AsTracking()
            .Include(r => r.Lines)
            .SingleOrDefaultAsync(r => r.Id == returnId);

        if (returnEntity is null)
        {
            return new Response { Message = $"Return {returnId} was not found." };
        }

        if (returnEntity.State != ReturnState.Registered)
        {
            return new Response { Message = $"Only returns of state {ReturnState.Registered} can be edited, current state: {returnEntity.State}." };
        }

        var returnLineIdsMissing = returnLineIds
            .Except(
                returnEntity.Lines.Select(l => l.Id)
            )
            .ToList();

        if (returnLineIdsMissing.Any())
        {
            return new Response
            {
                Message = "One or more return lines were not found.",
                Messages = returnLineIdsMissing.Select(rli => $"Return line {rli} was not found.")
            };
        }

        var returnLines = returnEntity.Lines
            .IntersectBy(returnLineIds, l => l.Id)
            .ToList();

        if (returnLines.Any(rl => rl.State != ReturnLineState.Registered))
        {
            return new Response
            {
                Message = "One or more lines are in invalid state.",
                Messages = returnLines
                    .Where(rl => rl.State != ReturnLineState.Registered)
                    .Select(rl =>
                        $"Return line {rl.Id} state {rl.State} is not valid for invoicing, return line must be in {ReturnLineState.Registered} state."
                    )
            };
        }

        foreach (var returnLine in returnLines)
        {
            returnLine.InvoiceNumberReturn = returnLine.Id.ToString("D10"); // MOCK
            returnLine.State = ReturnLineState.Invoiced;
        }

        if (returnEntity.Lines.All(rl => rl.State != ReturnLineState.Registered))
        {
            if (returnEntity.Lines.All(rl => rl.State == ReturnLineState.Declined))
            {
                returnEntity.State = ReturnState.Declined;
            }
            else if (returnEntity.Lines.All(rl => rl.State == ReturnLineState.Invoiced))
            {
                returnEntity.State = ReturnState.Invoiced;
            }
            else
            {
                returnEntity.State = ReturnState.InvoicedPartially;
            }
        }

        await _dbContext.SaveChangesAsync();

        return new Response { Success = true };
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
            return new ValueResponse<Domain.Entities.ReturnLine> { Message = $"Return {returnId} was not found." };
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
            return new ValueResponse<Domain.Entities.ReturnLine> { Message = $"Return {returnId} line {returnLineCandidate.Id} was not found." };
        }

        var deliveryPoint = await _customerService.GetDeliveryPointAsync(returnEntity.DeliveryPointId);

        if (deliveryPoint is null)
        {
            return new ValueResponse<Domain.Entities.ReturnLine> { Message = $"Delivery point {returnEntity.DeliveryPointId} was not found." };
        }

        var country = await _regionService.GetCountryAsync(deliveryPoint.CountryId);

        var returnCandidate = _mapper.Map<Return>(returnEntity);

        var returnLineExisting = returnCandidate.Lines.Single(l => l.Id == returnLineCandidate.Id);

        returnLineExisting.FeeConfigurationGroupIdDamagePackage = returnLineCandidate.FeeConfigurationGroupIdDamagePackage;
        returnLineExisting.FeeConfigurationGroupIdDamageProduct = returnLineCandidate.FeeConfigurationGroupIdDamageProduct;
        returnLineExisting.Note = returnLineCandidate.Note;
        returnLineExisting.Quantity = returnLineCandidate.Quantity;

        var invoiceLines = await _invoiceService
            .FilterLinesAsync(
                deliveryPoint.CustomerId,
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
                    returnValidated.Lines.SelectMany(
                        l => l.Messages,
                        (l, m) => $"Line {l.Reference}: {m}"
                    )
                )
            };
        }

        var returnEstimated = await _returnFeeService.ResolveAsync(returnValidated, deliveryPoint, country, invoiceLines);

        var response = await _returnService.MergeAsync(
            _returnFeeService.Calculate(returnEstimated, invoiceLines)
        );

        if (response is not { Success: true, Value: not null })
        {
            return new ValueResponse<Domain.Entities.ReturnLine> { Message = response.Message, Messages = response.Messages };
        }

        var returnLine = response.Value.Lines.Single(l => l.Id == returnLineCandidate.Id);

        await _dbContext.SaveChangesAsync();

        return new ValueResponse<Domain.Entities.ReturnLine> { Success = true, Value = returnLine };
    }
}
