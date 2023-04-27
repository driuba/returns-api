using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Returns.Domain.Mock;
using Returns.Logic.Mock.Repositories;

namespace Returns.Api.Mock.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Route("/mock/regions")]
public class RegionsController : ControllerBase
{
    private readonly MockDbContext _dbContext;

    public RegionsController(MockDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        return Ok(
            await _dbContext
                .Set<Region>()
                .ToListAsync()
        );
    }
}
