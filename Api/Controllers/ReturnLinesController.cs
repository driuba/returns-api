// ReSharper disable RouteTemplates.ActionRoutePrefixCanBeExtractedToControllerRoute

using AutoMapper;
using AutoMapper.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Returns.Domain.Api;
using Returns.Domain.Services;
using Returns.Logic.Repositories;

namespace Returns.Api.Controllers;

public class ReturnLinesController : ControllerBase
{
    private readonly ReturnDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IReturnLineService _returnLineService;

    public ReturnLinesController(ReturnDbContext dbContext, IMapper mapper, IReturnLineService returnLineService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _returnLineService = returnLineService;
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

    [HttpPost("returns({returnId:int})/lines")]
    public async Task<IActionResult> Post(string companyId, int returnId, ReturnLineRequest request)
    {
        var response = await _returnLineService.CreateAsync(returnId, _mapper.Map<Domain.Dto.ReturnLine>(request));

        if (response.Success)
        {
            return Created(
                _mapper.Map<ReturnLine>(response.Value)
            );
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
