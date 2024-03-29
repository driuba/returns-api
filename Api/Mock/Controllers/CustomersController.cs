using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Returns.Domain.Mock;
using Returns.Domain.Mock.Api;
using Returns.Domain.Services;
using Returns.Logic.Mock.Repositories;

namespace Returns.Api.Mock.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Route("/mock/{companyId}/customers")]
public class CustomersController : ControllerBase
{
    private readonly MockDbContext _dbContext;
    private readonly ISessionService _sessionService;

    public CustomersController(MockDbContext dbContext, ISessionService sessionService)
    {
        _dbContext = dbContext;
        _sessionService = sessionService;
    }

    [HttpPost("filter")]
    public async Task<IActionResult> Filter(string companyId, FilterCustomersRequest request)
    {
        return Ok(
            await _dbContext
                .Set<Customer>()
                .Where(c =>
                    string.IsNullOrEmpty(c.ParentId)
                        ? c.Companies.Any(cc => cc.CompanyId == _sessionService.CompanyId)
                        : c.Parent!.Companies.Any(cc => cc.CompanyId == _sessionService.CompanyId)
                )
                .Where(c =>
                    string.IsNullOrEmpty(_sessionService.CustomerId) ||
                    c.Id == _sessionService.CustomerId ||
                    c.ParentId == _sessionService.CustomerId
                )
                .Where(c => request.CustomerIds.Contains(c.Id))
                .Select(c => new { c.Id, c.Name, c.ParentId })
                .ToListAsync()
        );
    }

    [HttpGet("")]
    public async Task<IActionResult> Get(string companyId, bool parent = false, string? search = null)
    {
        return Ok(
            await _dbContext
                .Set<Customer>()
                .Where(c =>
                    string.IsNullOrEmpty(c.ParentId)
                        ? c.Companies.Any(cc => cc.CompanyId == _sessionService.CompanyId)
                        : c.Parent!.Companies.Any(cc => cc.CompanyId == _sessionService.CompanyId)
                )
                .Where(c =>
                    string.IsNullOrEmpty(_sessionService.CustomerId) ||
                    c.Id == _sessionService.CustomerId ||
                    c.ParentId == _sessionService.CustomerId
                )
                .Where(c => !parent || string.IsNullOrEmpty(c.ParentId))
                .Where(c =>
                    string.IsNullOrEmpty(search) ||
                    c.Id.Contains(search) ||
                    c.Name.Contains(search)
                )
                .Select(c => new { c.Id, c.Name, c.ParentId })
                .ToListAsync()
        );
    }
}
