using AutoMapper;
using AutoMapper.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Returns.Domain.Api;
using Returns.Logic.Repositories;

namespace Returns.Api.Controllers;

public class FeeConfigurationGroupsController : ControllerBase
{
    private readonly ReturnDbContext _dbContext;
    private readonly IMapper _mapper;

    public FeeConfigurationGroupsController(ReturnDbContext dbContext, IMapper mapper)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    [HttpGet("feeConfigurationGroups")]
    public async Task<IActionResult> Get(string companyId, ODataQueryOptions<FeeConfigurationGroup> options)
    {
        return Ok(
            await _dbContext
                .Set<Domain.Entities.FeeConfigurationGroup>()
                .GetQueryAsync(_mapper, options)
        );
    }

    [HttpGet("feeConfigurationGroups({id:int})")]
    public async Task<IActionResult> Get(string companyId, int id, ODataQueryOptions<FeeConfigurationGroup> options)
    {
        var query = await _dbContext
            .Set<Domain.Entities.FeeConfigurationGroup>()
            .Where(fcg => fcg.Id == id)
            .GetQueryAsync(_mapper, options);

        var feeConfigurationGroup = await query.SingleOrDefaultAsync();

        if (feeConfigurationGroup is null)
        {
            return NotFound();
        }

        return Ok(feeConfigurationGroup);
    }
}
