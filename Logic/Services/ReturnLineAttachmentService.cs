using Microsoft.EntityFrameworkCore;
using Returns.Domain.Dto;
using Returns.Domain.Entities;
using Returns.Domain.Enums;
using Returns.Domain.Services;
using Returns.Logic.Repositories;

namespace Returns.Logic.Services;

public class ReturnLineAttachmentService : IReturnLineAttachmentService
{
    private readonly ReturnDbContext _dbContext;
    private readonly IStorageService _storageService;

    public ReturnLineAttachmentService(ReturnDbContext dbContext, IStorageService storageService)
    {
        _dbContext = dbContext;
        _storageService = storageService;
    }

    public async Task<ValueResponse<ReturnLineAttachment>> CreateAsync(int returnId, int returnLineId, Stream file, string name)
    {
        var returnEntity = await _dbContext
            .Set<Domain.Entities.Return>()
            .Include(r => r.Lines.Where(l => l.Id == returnLineId))
            .SingleOrDefaultAsync(r => r.Id == returnId);

        if (returnEntity is null)
        {
            return new ValueResponse<ReturnLineAttachment>
            {
                Message = $"Return {returnId} was not found."
            };
        }

        if (returnEntity.State != ReturnState.New)
        {
            return new ValueResponse<ReturnLineAttachment>
            {
                Message = $"Only returns of state {ReturnState.New} can be edited, current state: {returnEntity.State}."
            };
        }

        var returnLine = returnEntity.Lines.SingleOrDefault();

        if (returnLine is null)
        {
            return new ValueResponse<ReturnLineAttachment>
            {
                Message = $"Return {returnId} line {returnLineId} was not found."
            };
        }

        var response = await _storageService.CreateAsync(file);

        if (response is not { Success: true, Value: not null })
        {
            return new ValueResponse<ReturnLineAttachment>
            {
                Message = response.Message,
                Messages = response.Messages
            };
        }

        var returnLineAttachment = new ReturnLineAttachment(name)
        {
            ReturnLineId = returnLine.Id,
            StorageId = response.Value.Value
        };

        _dbContext
            .Set<ReturnLineAttachment>()
            .Add(returnLineAttachment);

        await _dbContext.SaveChangesAsync();

        return new ValueResponse<ReturnLineAttachment>
        {
            Success = true,
            Value = returnLineAttachment
        };
    }

    public async Task<ValueResponse<Attachment>> GetAsync(int returnId, int returnLineId, Guid storageId)
    {
        var returnEntity = await _dbContext
            .Set<Domain.Entities.Return>()
            .Include(r => r.Lines.Where(l => l.Id == returnLineId))
            .ThenInclude(l => l.Attachments.Where(a => a.StorageId == storageId))
            .SingleOrDefaultAsync(r => r.Id == returnId);

        if (returnEntity is null)
        {
            return new ValueResponse<Attachment>
            {
                Message = $"Return {returnId} was not found."
            };
        }

        if (returnEntity.State != ReturnState.New)
        {
            return new ValueResponse<Attachment>
            {
                Message = $"Only returns of state {ReturnState.New} can be edited, current state: {returnEntity.State}."
            };
        }

        var returnLine = returnEntity.Lines.SingleOrDefault();

        if (returnLine is null)
        {
            return new ValueResponse<Attachment>
            {
                Message = $"Return {returnId} line {returnLineId} was not found."
            };
        }

        var returnLineAttachment = returnLine.Attachments.SingleOrDefault();

        if (returnLineAttachment is null)
        {
            return new ValueResponse<Attachment>
            {
                Message = $"Return {returnId} line {returnLineId} attachment {storageId} was not found."
            };
        }

        var response = _storageService.Get(returnLineAttachment.StorageId);

        if (response is not { Success: true, Value: not null })
        {
            return new ValueResponse<Attachment>
            {
                Message = $"Return {returnId} line {returnLineId} attachment {storageId} file was not found."
            };
        }

        return new ValueResponse<Attachment>
        {
            Success = true,
            Value = new Attachment(response.Value, returnLineAttachment.Name)
        };
    }

    public async Task<ValueResponse<ReturnLineAttachment>> DeleteAsync(int returnId, int returnLineId, Guid storageId)
    {
        var returnEntity = await _dbContext
            .Set<Domain.Entities.Return>()
            .AsTracking()
            .Include(r => r.Lines.Where(l => l.Id == returnLineId))
            .ThenInclude(l => l.Attachments.Where(a => a.StorageId == storageId))
            .SingleOrDefaultAsync(r => r.Id == returnId);

        if (returnEntity is null)
        {
            return new ValueResponse<ReturnLineAttachment>
            {
                Message = $"Return {returnId} was not found."
            };
        }

        if (returnEntity.State != ReturnState.New)
        {
            return new ValueResponse<ReturnLineAttachment>
            {
                Message = $"Only returns of state {ReturnState.New} can be edited, current state: {returnEntity.State}."
            };
        }

        var returnLine = returnEntity.Lines.SingleOrDefault();

        if (returnLine is null)
        {
            return new ValueResponse<ReturnLineAttachment>
            {
                Message = $"Return {returnId} line {returnLineId} was not found."
            };
        }

        var returnLineAttachment = returnLine.Attachments.SingleOrDefault();

        if (returnLineAttachment is null)
        {
            return new ValueResponse<ReturnLineAttachment>
            {
                Message = $"Return {returnId} line {returnLineId} attachment {storageId} was not found."
            };
        }

        var response = _storageService.Delete(returnLineAttachment.StorageId);
        if (!response.Success)
        {
            return new ValueResponse<ReturnLineAttachment>
            {
                Message = response.Messages.SingleOrDefault(),
            };
        }

        _dbContext
            .Set<ReturnLineAttachment>()
            .Remove(returnLineAttachment);

        await _dbContext.SaveChangesAsync();

        return new ValueResponse<ReturnLineAttachment>
        {
            Success = true,
            Value = returnLineAttachment
        };
    }
}
