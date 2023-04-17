using AutoMapper;
using AutoMapper.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Returns.Domain.Api;
using Returns.Logic.Repositories;

namespace Returns.Api.Controllers;

public class ReturnAvailabilitiesController : ControllerBase
{
    private readonly ReturnDbContext _dbContext;
    private readonly IMapper _mapper;

    public ReturnAvailabilitiesController(ReturnDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet("returns/availabilities")]
    public async Task<IActionResult> Get(string companyId, ODataQueryOptions<ReturnAvailability> options)
    {
        return Ok(
            await _dbContext
                .Set<ReturnAvailability>()
                .GetQueryAsync(_mapper, options)
        );
    }
}
