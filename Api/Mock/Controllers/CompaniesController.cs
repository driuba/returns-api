using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Returns.Domain.Mock;
using Returns.Domain.Services;
using Returns.Logic.Mock.Repositories;

namespace Returns.Api.Mock.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Route("/mock/companies")]
public class CompaniesController : ControllerBase
{
    private readonly MockDbContext _dbContext;
    private readonly ISessionService _sessionService;

    public CompaniesController(MockDbContext dbContext, ISessionService sessionService)
    {
        _dbContext = dbContext;
        _sessionService = sessionService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        if (!string.IsNullOrEmpty(_sessionService.CustomerId))
        {
            return Ok(
                await _dbContext
                    .Set<CompanyCustomer>()
                    .Where(cc => cc.CustomerId == _sessionService.CustomerId)
                    .Select(cc => cc.Company)
                    .Select(c => new { c.Id })
                    .ToListAsync()
            );
        }

        return Ok(
            await _dbContext
                .Set<Company>()
                .Select(c => new { c.Id })
                .ToListAsync()
        );
    }
}
