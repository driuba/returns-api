using AutoMapper;
using AutoMapper.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Returns.Domain.Api;
using Returns.Domain.Services;
using Returns.Logic.Repositories;

namespace Returns.Api.Controllers;

public class ReturnsController : ControllerBase
{
    private readonly ReturnDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IReturnService _returnService;

    public ReturnsController(ReturnDbContext dbContext, IMapper mapper, IReturnService returnService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _returnService = returnService;
    }

    [HttpDelete("returns({id:int})")]
    public async Task<IActionResult> Delete(string companyId, int id)
    {
        var response = await _returnService.Delete(id);

        if (response.Success)
        {
            return NoContent();
        }

        return BadRequest(
            _mapper.Map<Response>(response)
        );
    }

    [HttpPost("returns/estimate")]
    public async Task<IActionResult> Estimate(string companyId, ReturnRequest returnCandidate)
    {
        var returnEstimated = await _returnService.Estimate(
            _mapper.Map<Domain.Dto.Return>(returnCandidate)
        );

        return StatusCode(
            returnEstimated.Messages.Any() || returnEstimated.Lines.Any(l => l.Messages.Any())
                ? StatusCodes.Status400BadRequest
                : StatusCodes.Status200OK,
            _mapper.Map<ReturnEstimated>(returnEstimated)
        );
    }

    [HttpGet("returns")]
    public async Task<IActionResult> Get(string companyId, ODataQueryOptions<Return> options)
    {
        return Ok(
            await _dbContext
                .Set<Domain.Entities.Return>()
                .GetQueryAsync(_mapper, options)
        );
    }

    [HttpGet("returns({id:int})")]
    public async Task<IActionResult> Get(string companyId, int id, ODataQueryOptions<Return> options)
    {
        var query = await _dbContext
            .Set<Domain.Entities.Return>()
            .Where(r => r.Id == id)
            .GetQueryAsync(_mapper, options);

        var returnEntry = await query.SingleOrDefaultAsync();

        if (returnEntry is null)
        {
            return NotFound();
        }

        return Ok(returnEntry);
    }

    [HttpPatch("returns({id:int})")]
    public async Task<IActionResult> Patch(string companyId, int id, Delta<Return> delta)
    {
        var entity = await _dbContext
            .Set<Domain.Entities.Return>()
            .SingleOrDefaultAsync(r => r.Id == id);

        if (entity is null)
        {
            return NotFound();
        }

        var returnCandidate = _mapper.Map<Return>(entity);

        delta.Patch(returnCandidate);

        _mapper.Map(returnCandidate, entity);

        var response = await _returnService.Update(entity);

        if (response.Success)
        {
            return Updated(
                _mapper.Map<Return>(response.Value)
            );
        }

        return BadRequest(
            _mapper.Map<Response>(response)
        );
    }

    [HttpPost("returns")]
    public async Task<IActionResult> Post(string companyId, ReturnRequest returnCandidate)
    {
        var response = await _returnService.Create(
            _mapper.Map<Domain.Dto.Return>(returnCandidate)
        );

        if (response.Success)
        {
            return Created(
                _mapper.Map<Return>(response.Value)
            );
        }

        return BadRequest(
            _mapper.Map<Response>(response)
        );
    }
}
