using AutoMapper;
using AutoMapper.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Returns.Domain.Api;
using Returns.Domain.Services;
using Returns.Logic.Repositories;

namespace Returns.Api.Controllers;

public class ReturnAvailabilitiesController : ControllerBase
{
    private readonly ReturnDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IReturnAvailabilityService _returnAvailabilityService;

    public ReturnAvailabilitiesController(ReturnDbContext dbContext, IMapper mapper, IReturnAvailabilityService returnAvailabilityService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _returnAvailabilityService = returnAvailabilityService;
    }

    [HttpGet("returnAvailabilities/filter(deliveryPointId={deliveryPointId})")]
    public async Task<IActionResult> Get(string companyId, string deliveryPointId, ODataQueryOptions<ReturnAvailability> options)
    {
        var response = await _returnAvailabilityService.GetAsync(deliveryPointId);

        if (response is not { Success: true, Value: not null })
        {
            return BadRequest(
                _mapper.Map<Response>(response)
            );
        }

        var returnAvailability = new[] { response.Value }
            .AsQueryable()
            .GetQuery(_mapper, options)
            .SingleOrDefault();

        if (returnAvailability is null)
        {
            return NotFound();
        }

        return Ok(
            _mapper.Map<ReturnAvailability>(returnAvailability)
        );

    }

    [HttpGet("returnAvailabilities")]
    public async Task<IActionResult> Get(string companyId, ODataQueryOptions<ReturnAvailability> options)
    {
        return Ok(
            await _dbContext
                .Set<ReturnAvailability>()
                .GetQueryAsync(_mapper, options)
        );
    }
}
