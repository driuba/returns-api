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

public class FeeConfigurationsController : ControllerBase
{
    private readonly ReturnDbContext _dbContext;
    private readonly IFeeConfigurationService _feeConfigurationService;
    private readonly IMapper _mapper;

    public FeeConfigurationsController(
        ReturnDbContext dbContext,
        IFeeConfigurationService feeConfigurationService,
        IMapper mapper
    )
    {
        _dbContext = dbContext;
        _feeConfigurationService = feeConfigurationService;
        _mapper = mapper;
    }

    [HttpDelete("feeConfigurations({id:int})")]
    public async Task<IActionResult> Delete(string companyId, int id)
    {
        var response = await _feeConfigurationService.Delete(id);

        if (response.Success)
        {
            return NoContent();
        }

        return BadRequest(_mapper.Map<Response>(response));
    }

    [HttpGet("feeConfigurations")]
    public async Task<IActionResult> Get(string companyId, ODataQueryOptions<FeeConfiguration> options)
    {
        return Ok(
            await _dbContext
                .Set<Domain.Entities.FeeConfiguration>()
                .GetQueryAsync(_mapper, options)
        );
    }

    [HttpGet("feeConfigurations({id:int})")]
    public async Task<IActionResult> Get(string companyId, int id, ODataQueryOptions<FeeConfiguration> options)
    {
        var query = await _dbContext
            .Set<Domain.Entities.FeeConfiguration>()
            .Where(fc => fc.Id == id)
            .GetQueryAsync(_mapper, options);

        var feeConfiguration = await query.SingleOrDefaultAsync();

        if (feeConfiguration is null)
        {
            return NotFound();
        }

        return Ok(feeConfiguration);
    }

    [HttpPatch("feeConfigurations({id:int})")]
    public async Task<IActionResult> Patch(string companyId, int id, Delta<FeeConfiguration> delta)
    {
        var entity = await _dbContext
            .Set<Domain.Entities.FeeConfiguration>()
            .SingleOrDefaultAsync(fc => fc.Id == id);

        if (entity is null)
        {
            return NotFound();
        }

        var feeConfiguration = _mapper.Map<FeeConfiguration>(entity);

        delta.Patch(feeConfiguration);

        _mapper.Map(feeConfiguration, entity);

        var response = await _feeConfigurationService.Update(entity);

        if (response.Success)
        {
            return Updated(
                _mapper.Map<FeeConfiguration>(response.Value)
            );
        }

        return BadRequest(
            _mapper.Map<Response>(response)
        );
    }

    [HttpPost("feeConfigurations")]
    public async Task<IActionResult> Post(string companyId, FeeConfiguration feeConfiguration)
    {
        var response = await _feeConfigurationService.Create(
            _mapper.Map<Domain.Entities.FeeConfiguration>(feeConfiguration)
        );

        if (response.Success)
        {
            return Created(
                _mapper.Map<FeeConfiguration>(response.Value)
            );
        }

        return BadRequest(
            _mapper.Map<Response>(response)
        );
    }
}