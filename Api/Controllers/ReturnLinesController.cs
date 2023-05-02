// ReSharper disable RouteTemplates.ActionRoutePrefixCanBeExtractedToControllerRoute

using AutoMapper;
using AutoMapper.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Returns.Domain.Api;
using Returns.Domain.Constants;
using Returns.Domain.Services;
using Returns.Logic.Repositories;

namespace Returns.Api.Controllers;

public class ReturnLinesController : ControllerBase
{
    private readonly ReturnDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IReturnLineService _returnLineService;
    private readonly ISessionService _sessionService;

    public ReturnLinesController(
        ReturnDbContext dbContext,
        IMapper mapper,
        IReturnLineService returnLineService,
        ISessionService sessionService
    )
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _returnLineService = returnLineService;
        _sessionService = sessionService;
    }

    [Authorize(AuthorizationPolicies.Admin)]
    [HttpPost("returns({returnId})/lines/decline")]
    public async Task<IActionResult> Decline(string companyId, int returnId, DeclineReturnLinesRequest request)
    {
        var response = await _returnLineService.DeclineAsync(returnId, request.Ids, request.Note);

        if (response.Success)
        {
            return NoContent();
        }

        return BadRequest(
            _mapper.Map<Response>(response)
        );
    }

    [HttpDelete("returns({returnId:int})/lines({returnLineId:int})")]
    public async Task<IActionResult> Delete(string companyId, int returnId, int returnLineId)
    {
        var response = await _returnLineService.DeleteAsync(returnId, returnLineId);

        if (response.Success)
        {
            return NoContent();
        }

        return BadRequest(
            _mapper.Map<Response>(response)
        );
    }

    [HttpGet("returns({returnId:int})/lines")]
    public async Task<IActionResult> Get(string companyId, int returnId, ODataQueryOptions<ReturnLine> options)
    {
        return Ok(
            await _dbContext
                .Set<Domain.Entities.ReturnLine>()
                .Where(rl => rl.ReturnId == returnId)
                .GetQueryAsync(_mapper, options)
        );
    }

    [HttpGet("returns({returnId:int})/lines({returnLineId:int})")]
    public async Task<IActionResult> Get(string companyId, int returnId, int returnLineId, ODataQueryOptions<ReturnLine> options)
    {
        var query = await _dbContext
            .Set<Domain.Entities.ReturnLine>()
            .Where(rl => rl.ReturnId == returnId)
            .Where(rl => rl.Id == returnLineId)
            .GetQueryAsync(_mapper, options);

        var returnLine = await query.SingleOrDefaultAsync();

        if (returnLine is null)
        {
            return NotFound();
        }

        return Ok(returnLine);
    }

    [Authorize(AuthorizationPolicies.Admin)]
    [HttpPost("returns({returnId:int})/lines/invoice")]
    public async Task<IActionResult> Invoice(string companyId, int returnId, InvoiceReturnLinesRequest request)
    {
        var response = await _returnLineService.InvoiceAsync(returnId, request.Ids);

        if (response.Success)
        {
            return NoContent();
        }

        return BadRequest(
            _mapper.Map<Response>(response)
        );
    }

    [HttpPost("returns({returnId:int})/lines")]
    public async Task<IActionResult> Post(string companyId, int returnId, ReturnLinesRequest request)
    {
        var response = await _returnLineService.CreateAsync(
            returnId,
            _mapper.Map<IEnumerable<Domain.Dto.ReturnLine>>(
                request.Lines,
                moo => moo.Items["applyRegistrationFee"] = _sessionService.Principal.IsInRole(Roles.Admin)
            )
        );

        if (response.Success)
        {
            return NoContent();
        }

        return BadRequest(
            _mapper.Map<Response>(response)
        );
    }

    [HttpPut("returns({returnId:int})/lines({returnLineId:int})")]
    public async Task<IActionResult> Put(string companyId, int returnId, int returnLineId, ReturnLineRequest request)
    {
        var response = await _returnLineService.UpdateAsync(
            returnId,
            _mapper.Map<Domain.Dto.ReturnLine>(
                request,
                moo => moo.Items["returnLineId"] = returnLineId
            )
        );

        if (response.Success)
        {
            return Updated(
                _mapper.Map<ReturnLine>(response.Value)
            );
        }

        return BadRequest(
            _mapper.Map<Response>(response)
        );
    }
}
