using Microsoft.EntityFrameworkCore;
using Returns.Domain.Dto;
using Returns.Domain.Enums;
using Returns.Domain.Services;
using Returns.Logic.Repositories;

namespace Returns.Logic.Services;

public class ReturnService : IReturnService
{
    private readonly ICustomerService _customerService;
    private readonly ReturnDbContext _dbContext;
    private readonly IRegionService _regionService;
    private readonly IStorageService _storageService;

    public ReturnService(
        ICustomerService customerService,
        ReturnDbContext dbContext,
        IRegionService regionService,
        IStorageService storageService
    )
    {
        _customerService = customerService;
        _dbContext = dbContext;
        _regionService = regionService;
        _storageService = storageService;
    }

    public async Task<ValueResponse<Domain.Entities.Return>> Create(Return returnCandidate)
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
            .Where(si => si.HasValue)
            .Cast<Guid>()
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
        throw new NotImplementedException();
    }
}
