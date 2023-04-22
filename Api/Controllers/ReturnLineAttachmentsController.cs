// ReSharper disable RouteTemplates.ActionRoutePrefixCanBeExtractedToControllerRoute

using System.Net.Mime;
using AutoMapper;
using AutoMapper.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Returns.Domain.Api;
using Returns.Domain.Services;
using Returns.Logic.Repositories;

namespace Returns.Api.Controllers;

public class ReturnLineAttachmentsController : ControllerBase
{
    private readonly ReturnDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IReturnLineAttachmentService _returnLineAttachmentService;

    public ReturnLineAttachmentsController(ReturnDbContext dbContext, IMapper mapper, IReturnLineAttachmentService returnLineAttachmentService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _returnLineAttachmentService = returnLineAttachmentService;
    }

    [HttpDelete("returns({returnId:int})/lines({returnLineId:int})/attachments({storageId:guid})")]
    public async Task<IActionResult> Delete(string companyId, int returnId, int returnLineId, Guid storageId)
    {
        var response = await _returnLineAttachmentService.DeleteAsync(returnId, returnLineId, storageId);

        if (response.Success)
        {
            return NoContent();
        }

        return BadRequest(
            _mapper.Map<Response>(response)
        );
    }

    [HttpGet("returns({returnId:int})/lines({returnLineId:int})/attachments")]
    public async Task<IActionResult> Get(string companyId, int returnId, int returnLineId, ODataQueryOptions<ReturnLineAttachment> options)
    {
        return Ok(
            await _dbContext
                .Set<ReturnLineAttachment>()
                .Where(rla => rla.Line.ReturnId == returnId)
                .Where(rla => rla.Line.Id == returnLineId)
                .GetQueryAsync(_mapper, options)
        );
    }

    [HttpGet("returns({returnId:int})/lines({returnLineId:int})/attachments({storageId:guid})")]
    [Produces(MediaTypeNames.Application.Octet, MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Get(string companyId, int returnId, int returnLineId, Guid storageId)
    {
        var response = await _returnLineAttachmentService.GetAsync(returnId, returnLineId, storageId);

        if (response is { Success: true, Value: not null })
        {
            return File(response.Value.File, MediaTypeNames.Application.Octet, response.Value.Name);
        }

        return BadRequest(
            _mapper.Map<Response>(response)
        );
    }

    [Consumes(Domain.Constants.MediaTypeNames.Multipart.FormData)]
    [HttpPost("returns({returnId:int})/lines({returnLineId:int})/attachments")]
    public async Task<IActionResult> Post(string companyId, int returnId, int returnLineId, [FromForm] AttachmentRequest request)
    {
        await using var file = request.File.OpenReadStream();

        var response = await _returnLineAttachmentService.CreateAsync(returnId, returnLineId, file, request.FileName);

        if (response.Success)
        {
            return Created(
                _mapper.Map<ReturnLineAttachment>(response.Value)
            );
        }

        return BadRequest(
            _mapper.Map<Response>(response)
        );
    }
}
